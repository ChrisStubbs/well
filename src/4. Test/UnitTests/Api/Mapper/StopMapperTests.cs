﻿using Moq;
using PH.Well.Common;
using PH.Well.Common.Contracts;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;

namespace PH.Well.UnitTests.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Factories;
    using NUnit.Framework;
    using Well.Api.Mapper;
    using Well.Domain;
    //using Well.Domain.Enums;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class StopMapperTests
    {

        private readonly Branch branch = new BranchFactory().Build();
        private readonly RouteHeader routeHeader = new RouteHeaderFactory().Build();
        private List<Branch> branches;
        private StopMapper mapper;
        private Mock<IUserNameProvider> userNameProvider = new Mock<IUserNameProvider>();
        private Mock<IJobService> jobService = new Mock<IJobService>();
        private Mock<ILookupService> lookupService = new Mock<ILookupService>();

        [SetUp]
        public void SetUp()
        {
            mapper = new StopMapper(jobService.Object, userNameProvider.Object, lookupService.Object);
            branches = new List<Branch> { branch };

            var jobTypes = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(((int)JobType.Alcohol).ToString(), JobType.Alcohol.ToString()),
                new KeyValuePair<string, string>(((int)JobType.Tobacco).ToString(), JobType.Tobacco.ToString())
            };

            lookupService.Setup(p => p.GetLookup(LookupType.JobType)).Returns(jobTypes);
        }

        [Test]
        public void ShouldMapStopModelHeaderItems()
        {
            var stop = new StopFactory().Build();
            var job = new JobFactory()
                .WithTotalShort(10)
                .WithOuterDiscrepancyFound(true)
                .WithOuterCount(1)
                .Build();

            var stopModel = mapper.Map(branches, routeHeader, stop, new List<Job> { job }, new List<Assignee>(), new List<JobDetailLineItemTotals>());

            Assert.That(stopModel.RouteId, Is.EqualTo(routeHeader.Id));
            Assert.That(stopModel.RouteNumber, Is.EqualTo(routeHeader.RouteNumber));
            Assert.That(stopModel.Branch, Is.EqualTo(branch.BranchName));
            Assert.That(stopModel.BranchId, Is.EqualTo(branch.Id));
            Assert.That(stopModel.Driver, Is.EqualTo(routeHeader.DriverName));
            Assert.That(stopModel.RouteDate, Is.EqualTo(routeHeader.RouteDate));
            Assert.That(stopModel.Tba, Is.EqualTo(10));
            Assert.That(stopModel.StopNo, Is.EqualTo(stop.PlannedStopNumber));
            Assert.That(stopModel.TotalNoOfStopsOnRoute, Is.EqualTo(routeHeader.PlannedStops));
        }

        [Test]
        public void ShouldMapOneItemForEachJobDetailItems()
        {
            var stop = new StopFactory().Build();
            var jobDetails1 = new List<JobDetail>
            {
                JobDetailFactory.New
                    .With(p => p.LineNumber = 1)
                    .With(p => p.PhProductCode = "Product")
                    .Build(),
                JobDetailFactory.New
                    .With(p => p.LineNumber = 2)
                    .With(p => p.PhProductCode = "Product2")
                    .Build()
            };

            var lineItems1 = new List<LineItem>();

            foreach (var item in jobDetails1)
            {
                lineItems1.Add(
                LineItemFactory.New
                    .With(p => p.LineNumber = item.LineNumber)
                    .With(p => p.ProductCode = item.PhProductCode)
                    .Build());
            }

            var jobDetails2 = new List<JobDetail>
            {
                new JobDetailFactory()
                .With(p => p.LineNumber = 1)
                .With(p => p.PhProductCode = "Product")
                .Build()
            };

            var lineItems2 = new List<LineItem>()
            {
                LineItemFactory.New
                .With(p => p.LineNumber = jobDetails2.First().LineNumber)
                .With(p => p.ProductCode = jobDetails2.First().PhProductCode)
                .Build()
            };

            var job = new JobFactory()
                    .With(x => x.JobDetails = jobDetails1)
                    .With(x => x.LineItems = lineItems1)
                    .WithTotalShort(10)
                    .Build();

            var job2 = new JobFactory()
                .With(x => x.JobDetails = jobDetails2)
                .With(p => p.LineItems = lineItems2)
                .Build();

            var stopModel = mapper.Map(branches, routeHeader, stop, new List<Job> { job, job2 }, new List<Assignee>(), new List<JobDetailLineItemTotals>());

            Assert.That(stopModel.Items.Count, Is.EqualTo(3));

            stopModel = mapper.Map(branches,
                routeHeader,
                stop,
                new List<Job> { job, job2 },
                new List<Assignee>(),
                new List<JobDetailLineItemTotals> { new JobDetailLineItemTotals { JobDetailId = stopModel.Items.First().JobDetailId } });

            Assert.That(stopModel.Items.Count, Is.EqualTo(3));
        }

        [Test]
        public void ShouldMapOneItemForEachJobDetailItemsThatIsNotATobaccoBag()
        {
            const string eighteenCharacterBarcode = "124789547568752311";

            var stop = new StopFactory().Build();

            var jobDetails1 = new List<JobDetail>
            {
                new JobDetailFactory()
                    .With(x=> x.PhProductCode = eighteenCharacterBarcode)
                    .With(p => p.LineNumber = 1)
                    .With(p => p.PhProductCode = "P1")
                    .Build(),
                new JobDetailFactory()
                    .With(p => p.LineNumber = 2)
                    .With(p => p.PhProductCode = "P2")
                    .Build(),
                new JobDetailFactory()
                    .With(p => p.LineNumber = 3)
                    .With(p => p.PhProductCode = "P3")
                .Build()
            };

            var lineItems = new List<LineItem>();

            foreach (var item in jobDetails1)
            {
                lineItems.Add(LineItemFactory.New
                   .With(p => p.LineNumber = item.LineNumber)
                   .With(p => p.ProductCode = item.PhProductCode)
                   .Build());
            }

            var job = new JobFactory()
                .With(x => x.JobTypeCode = "DEL-TOB")
                .With(x => x.JobDetails = jobDetails1)
                .With(x => x.LineItems = lineItems)
                .With(x => x.ResolutionStatus = Well.Domain.Enums.ResolutionStatus.DriverCompleted)
                .WithTotalShort(10)
                .Build();

            var stopModel = mapper.Map(branches, routeHeader, stop, new List<Job> { job }, new List<Assignee>(), new List<JobDetailLineItemTotals>());

            Assert.That(stopModel.Items.Count, Is.EqualTo(3));
            Assert.False(stopModel.Items.Any(x => x.Product == eighteenCharacterBarcode));
        }

        [Test]
        public void ShouldIgnoreDocumentens()
        {
            var job = new JobFactory()
                        .With(x => x.JobTypeCode = "DEL-DOC")
                        .Build();

            var stopModel = mapper.Map(
                branches,
                routeHeader,
                new StopFactory().Build(),
                new List<Job> { job },
                new List<Assignee>(),
                new List<JobDetailLineItemTotals>());

            Assert.That(stopModel.Items.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldMapToStopModelItems()
        {
            var stop = new StopFactory().Build();
            var jobDetails1 = new List<JobDetail>
            {
                new JobDetailFactory()
                    .With(x=> x.PhProductCode="Product")
                    .With(x=> x.ProdDesc="ProductDescription")
                    .With(x=> x.NetPrice=72)
                    .With(x=> x.OriginalDespatchQty=592)
                    .With(x=> x.DeliveredQty=666)
                    .With(x=> x.JobDetailDamages.Add(new JobDetailDamage {Qty = 6}))
                    .With(x=> x.JobDetailDamages.Add(new JobDetailDamage {Qty = 5}))
                    .With(x=> x.ShortQty=22)
                    .With(x => x.LineDeliveryStatus = "Delivered")
                    .With(x => x.IsHighValue = true)
                    .With(x => x.SSCCBarcode="12478459554678952")
                    .With(x => x.LineNumber = 1)
                    .Build()
            };

            var job = new JobFactory()
                        .With(x => x.Id = 2545)
                        .With(x => x.InvoiceNumber = "INVNO1")
                        .With(x => x.JobTypeCode = JobTypeDescriptions.Description(JobType.Tobacco))
                        .With(x => x.JobTypeAbbreviation = "test")
                        .With(x => x.PhAccount = "PHAcccountNo")
                        .With(x => x.JobDetails = jobDetails1)
                        .With(j => j.LineItems = new List<LineItem>()
                        {
                            LineItemFactory.New.With(p => p.LineNumber = 1).With(p => p.ProductCode = "Product").Build()
                        })
                        .With(x => x.ResolutionStatus = Well.Domain.Enums.ResolutionStatus.DriverCompleted)
                        .WithTotalShort(10)
                        .Build();

            var totals = new List<JobDetailLineItemTotals>
            {
                new JobDetailLineItemTotals
                {
                    JobDetailId = job.JobDetails.First().Id,
                    ShortTotal = 33,
                    DamageTotal = 155
                }
            };
            var stopModel = mapper.Map(branches, routeHeader, stop, new List<Job> { job }, new List<Assignee>(), totals);

            Assert.That(stopModel.Items.Count, Is.EqualTo(1));
            var item = stopModel.Items[0];
            Assert.That(item.JobId, Is.EqualTo(2545));
            Assert.That(item.Invoice, Is.EqualTo(job.InvoiceNumber));
            Assert.That(item.InvoiceId, Is.EqualTo(job.ActivityId));
            Assert.That(item.Type, Is.EqualTo(job.JobType.ToString()));
            Assert.That(item.JobTypeAbbreviation, Is.EqualTo("test"));
            Assert.That(item.Account, Is.EqualTo("PHAcccountNo"));
            Assert.That(item.JobDetailId, Is.EqualTo(jobDetails1[0].Id));
            Assert.That(item.Product, Is.EqualTo(jobDetails1.First().PhProductCode));
            Assert.That(item.Description, Is.EqualTo("ProductDescription"));
            Assert.That(item.Value, Is.EqualTo(72));
            Assert.That(item.Invoiced, Is.EqualTo(592));
            Assert.That(item.Delivered, Is.EqualTo(666));
            Assert.That(item.Damages, Is.EqualTo(155));
            Assert.That(item.Shorts, Is.EqualTo(33));
            Assert.That(item.Checked, Is.True);
            Assert.That(item.HighValue, Is.True);
            Assert.That(item.BarCode, Is.EqualTo("12478459554678952"));
        }

        [Test]
        public void ShouldUseJobServiceToAssignCanEdit()
        {
            var job = JobFactory.New
                .With(j => j.JobDetails = new List<JobDetail>()
                    {
                        JobDetailFactory.New.With(p => p.LineNumber = 1).With(p => p.PhProductCode = "Product").Build()
                    })
                .With(j => j.LineItems = new List<LineItem>()
                    {
                        LineItemFactory.New.With(p => p.LineNumber = 1).With(p => p.ProductCode = "Product").Build()
                    })
                .Build();

            var result = mapper.Map(branches, routeHeader, StopFactory.New.Build(), new List<Job> { job },
                new List<Assignee>(),
                new List<JobDetailLineItemTotals>());

            jobService.Verify(x => x.CanEdit(job, It.IsAny<string>()), Times.Once);
        }
    }
}
