﻿namespace PH.Well.UnitTests.Services
{
    using NUnit.Framework;

    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.EpodServices;

    [TestFixture]
    public class RouteMapperTests
    {
        private RouteMapper mapper;

        [SetUp]
        public void Setup()
        {
            this.mapper = new RouteMapper();
        }

        [Test]
        public void RouteHeaderMapper()
        {
            var from = new RouteHeader();
            from.RouteStatusCode = "Compl";
            from.RouteStatusDescription = "Completed";
            from.PerformanceStatusCode = "InProg";
            from.PerformanceStatusDescription = "In Progress";
            from.AuthByPass = 11;
            from.NonAuthByPass = 45;
            from.ShortDeliveries = 3;
            from.DamagesRejected = 3;
            from.DamagesAccepted = 1;
         //   from.StartDepotCode = "NIR";
            from.ActualStopsCompleted = 800;

            var to = new RouteHeader();

            this.mapper.Map(from, to);

            Assert.That(to.RouteStatusCode, Is.EqualTo(from.RouteStatusCode));
            Assert.That(to.RouteStatusDescription, Is.EqualTo(from.RouteStatusDescription));
            Assert.That(to.PerformanceStatusCode, Is.EqualTo(from.PerformanceStatusCode));
            Assert.That(to.PerformanceStatusDescription, Is.EqualTo(from.PerformanceStatusDescription));
            Assert.That(to.AuthByPass, Is.EqualTo(from.AuthByPass));
            Assert.That(to.NonAuthByPass, Is.EqualTo(from.NonAuthByPass));
            Assert.That(to.ShortDeliveries, Is.EqualTo(from.ShortDeliveries));
            Assert.That(to.DamagesRejected, Is.EqualTo(from.DamagesRejected));
            Assert.That(to.DamagesAccepted, Is.EqualTo(from.DamagesAccepted));
//            Assert.That(to.StartDepotCode, Is.EqualTo(from.StartDepotCode));
            Assert.That(to.ActualStopsCompleted, Is.EqualTo(from.ActualStopsCompleted));
        }

        [Test]
        public void StopMapper()
        {
            var from = new StopDTO();

            from.StopStatusCode = "Something";
            from.StopStatusDescription = "Something else";
            from.PerformanceStatusCode = "not sure";
            from.PerformanceStatusDescription = "no i don't know";
            from.StopByPassReason = "Something";

            var to = new Stop();

            this.mapper.Map(from, to);

            Assert.That(to.StopStatusCode, Is.EqualTo(from.StopStatusCode));
            Assert.That(to.StopStatusDescription, Is.EqualTo(from.StopStatusDescription));
            Assert.That(to.PerformanceStatusCode, Is.EqualTo(from.PerformanceStatusCode));
            Assert.That(to.PerformanceStatusDescription, Is.EqualTo(from.PerformanceStatusDescription));
            Assert.That(to.StopByPassReason, Is.EqualTo(from.StopByPassReason));
        }

        [Test]
        public void StopUpdateMapper()
        {
            var from = new StopUpdate();

            from.PlannedStopNumber = "5";
            from.ShellActionIndicator = "A";

            var to = new Stop();

            this.mapper.Map(from, to);

            Assert.That(to.PlannedStopNumber, Is.EqualTo(from.PlannedStopNumber));
            Assert.That(to.ShellActionIndicator, Is.EqualTo(from.ShellActionIndicator));
        }

        [Test]
        public void JobMapper()
        {
            var from = new JobDTO();
            var to = new Job();

            from.JobByPassReason = "Some reason";
            from.PerformanceStatus = PerformanceStatus.Abypa;
            from.InvoiceNumber = "12009";

            this.mapper.Map(from, to);

            Assert.That(to.JobByPassReason, Is.EqualTo(from.JobByPassReason));
            Assert.That(to.PerformanceStatus, Is.EqualTo(from.PerformanceStatus));
            Assert.That(to.InvoiceNumber, Is.EqualTo(from.InvoiceNumber));
        }

        [Test]
        public void JobUpdateMapper()
        {
            var from = new JobUpdate();

            from.Sequence = 1;
            from.JobTypeCode = "A";
            from.PhAccount = "12000.001";
            from.PickListRef = "4433";
            from.InvoiceNumber = "223344";
            from.CustomerRef = "Ref";

            from.EntityAttributes.Add(new EntityAttribute { Code = "PICKED" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "ORDOUTERS" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "INVOUTERS" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "ALLOWSOCRD" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "COD" });
            from.EntityAttributes.Add(new EntityAttribute { Code = "ALLOWREORD" });
            
            var to = new Job();

            this.mapper.Map(from, to);

            Assert.That(to.Sequence, Is.EqualTo(from.Sequence));
            Assert.That(to.JobTypeCode, Is.EqualTo(from.JobTypeCode));
            Assert.That(to.PhAccount, Is.EqualTo(from.PhAccount));
            Assert.That(to.PickListRef, Is.EqualTo(from.PickListRef));
            Assert.That(to.InvoiceNumber, Is.EqualTo(from.InvoiceNumber));
            Assert.That(to.CustomerRef, Is.EqualTo(from.CustomerRef));
            Assert.That(to.PerformanceStatus, Is.EqualTo(PerformanceStatus.Notdef));
        }

        [Test]
        public void JobDetailMapper()
        {
            var from = new JobDetailDTO();

            from.ShortQty = 3;
            from.DeliveredQty = 5;

            var to = new JobDetail();

            this.mapper.Map(from, to);

            Assert.That(to.ShortQty, Is.EqualTo(from.ShortQty));
            Assert.That(to.DeliveredQty, Is.EqualTo(from.DeliveredQty));
        }

        [Test]
        public void JobDetailMapperInvertNegativeFigures()
        {
            var from = new JobDetailDTO
            {
                OriginalDespatchQty = -20,
                DeliveredQty = -15
            };

            var to = new JobDetail();

            mapper.Map(from, to);

            Assert.That(() => from.OriginalDespatchQty == (to.OriginalDespatchQty * -1));
            Assert.That(() => from.DeliveredQty == (to.DeliveredQty * -1));
        }

        [Test]
        public void JobDetailUpdateMapper()
        {
            var from = new JobDetailUpdate();

            from.PhProductCode = "2001";
            from.ProdDesc = "Mars";
            from.OrderedQty = 45;
            from.UnitMeasure = "3M";
            from.PhProductType = "Lubes";
            from.PackSize = "30";
            from.SingleOrOuter = "1";
            from.SSCCBarcode = "11222121";
            from.SkuGoodsValue = 3;

            var to = new JobDetail();

            this.mapper.Map(from, to);

            Assert.That(to.PhProductCode, Is.EqualTo(from.PhProductCode));
            Assert.That(to.ProdDesc, Is.EqualTo(from.ProdDesc));
            Assert.That(to.OrderedQty, Is.EqualTo(from.OrderedQty));
            Assert.That(to.UnitMeasure, Is.EqualTo(from.UnitMeasure));
            Assert.That(to.PhProductType, Is.EqualTo(from.PhProductType));
            Assert.That(to.PackSize, Is.EqualTo(from.PackSize));
            Assert.That(to.SingleOrOuter, Is.EqualTo(from.SingleOrOuter));
            Assert.That(to.SSCCBarcode, Is.EqualTo(from.SSCCBarcode));
            Assert.That(to.SkuGoodsValue, Is.EqualTo(from.SkuGoodsValue));
        }
    }
}