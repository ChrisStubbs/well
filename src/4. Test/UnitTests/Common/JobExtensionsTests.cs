using System;
using System.Linq;
using NUnit.Framework;
using PH.Well.Domain.Enums;
using PH.Well.Domain.Extensions;
using PH.Well.Domain.ValueObjects;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Common
{
    using Well.Domain;

    [TestFixture]
    class JobExtensionsTests
    {
        [Test]
        [Description("Check if the job HasUnresolvedActions")]
        [Category("Extension")]
        [Category("Job")]
        public void JobShouldHasUnresolvedActions()
        {
            var line = LineItemFactory.New
                .AddNotDefinedAction()
                .Build();

            var j = JobFactory.New
                    .With(p => p.LineItems.Add(line))
                    .Build();

            Assert.IsTrue(j.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the job doesn't  HasUnresolvedActions is credited")]
        [Category("Extension")]
        [Category("Job")]
        public void JobShouldNotHasUnresolvedActions()
        {
            var line = LineItemFactory.New
                .AddCreditAction()
                .Build();

            var j = JobFactory.New
                    .With(p => p.LineItems.Add(line))
                    .Build();

            Assert.IsFalse(j.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the job doesn't HasUnresolvedActions because it is closed")]
        [Category("Extension")]
        [Category("Job")]
        public void ClosedJobShouldNotHasUnresolvedActions()
        {
            var line = LineItemFactory.New
                .AddNotDefinedAction()
                .Build();

            var j = JobFactory.New
                    .With(p => p.LineItems.Add(line))
                    .With(p => p.ResolutionStatus = ResolutionStatus.Closed)
                    .Build();

            Assert.IsFalse(j.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the ActivitySourceDetail HasUnresolvedActions")]
        [Category("Extension")]
        [Category("ActivitySourceDetail")]
        public void ActivitySourceDetailShouldHasUnresolvedActions()
        {
            var sut = new ActivitySourceDetail
            {
                ResolutionStatus = ResolutionStatus.Approved,
                HasNoDefinedActions = true
            };

            Assert.IsTrue(sut.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the ActivitySourceDetail doesn't HasUnresolvedActions because is credited")]
        [Category("Extension")]
        [Category("ActivitySourceDetail")]
        public void ActivitySourceDetailShouldNotHasUnresolvedActions()
        {
            var sut = new ActivitySourceDetail
            {
                ResolutionStatus = ResolutionStatus.Credited,
                HasNoDefinedActions = false
            };

            Assert.IsFalse(sut.HasUnresolvedActions());
        }

        [Test]
        [Description("Check if the job doesn't HasUnresolvedActions because it is closed")]
        [Category("Extension")]
        [Category("ActivitySourceDetail")]
        public void ClosedActivitySourceDetailShouldNotHasUnresolvedActions()
        {
            var sut = new ActivitySourceDetail
            {
                ResolutionStatus = ResolutionStatus.Closed
            };

            Assert.IsFalse(sut.HasUnresolvedActions());
        }

        [Test]
        [Category("Extension")]
        [Category("Job")]
        public void ShouldReturnTheCorrectWellStatus()
        {
            foreach (var jobStatus in Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>())
            {
                switch (jobStatus)
                {
                    case JobStatus.NotDefined:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Unknown));
                        break;
                    case JobStatus.AwaitingInvoice:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Planned));
                        break;
                    case JobStatus.InComplete:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Invoiced));
                        break;
                    case JobStatus.Clean:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.Exception:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.Resolved:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.DocumentDelivery:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.CompletedOnPaper:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.Bypassed:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Bypassed));
                        break;
                    case JobStatus.Replanned:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Replanned));
                        break;
                    default:
                        Assert.IsTrue(false, "Add the new status ToWellStatus Method ");
                        break;
                }
            }
        }

        [Test]
        [Description("Check if job Identifier is been create correctly")]
        [Category("Extension")]
        [Category("Job")]
        public void ShouldCreateIdentifier()
        {
            var sut = JobFactory.New
                .With(p => p.PhAccount = "PhAccount")
                .With(p => p.PickListRef = "PickListRef")
                .With(p => p.JobTypeCode = "JobTypeCode")
                .Build();

            Assert.That($"{sut.PhAccount} - {sut.PickListRef} - {sut.JobTypeCode}", Is.EqualTo(sut.Identifier()));
        }

        [Test]
        [Description("Check if the job can be updated")]
        [Category("Extension")]
        [Category("Job")]
        public void CheckCanWeUpdateJobOnImport()
        {
            foreach (WellStatus item in System.Enum.GetValues(typeof(WellStatus)))
            {
                var sut = JobFactory.New
                    .With(p => p.WellStatus = item)
                    .Build();

                var testedValue = item != WellStatus.Complete;

                Assert.That(sut.CanWeUpdateJobOnImport(), Is.EqualTo(testedValue));
            }
        }
        
        [Test]
        [Description("Check if ReinstateJob Identifier is been create correctly")]
        [Category("Extension")]
        [Category("ReinstateJob")]
        public void ShouldCreateIdentifierForReinstateJob()
        {
            var sut = new ReinstateJob
            {
                PhAccount = "PhAccount",
                PickListRef = "PickListRef",
                JobTypeCode = "JobTypeCode"
            };

            Assert.That($"{sut.PhAccount} - {sut.PickListRef} - {sut.JobTypeCode}", Is.EqualTo(sut.Identifier()));
        }

        [Test]
        [Description("ReinstateJob Identifier And Job Identifier Should Be Equal")]
        [Category("Extension")]
        [Category("ReinstateJob")]
        public void ReinstateJob_And_Job_Identifier_Should_Be_Equal()
        {
            var sut = new ReinstateJob
            {
                PhAccount = "PhAccount",
                PickListRef = "PickListRef",
                JobTypeCode = "JobTypeCode"
            };

            var job = JobFactory.New
                .With(p => p.PhAccount = "PhAccount")
                .With(p => p.PickListRef = "PickListRef")
                .With(p => p.JobTypeCode = "JobTypeCode")
                .Build();

            Assert.That(job.Identifier(), Is.EqualTo(sut.Identifier()));
        }

        [Test]
        [Description("Check if the ReinstateJob can be updated")]
        [Category("Extension")]
        [Category("ReinstateJob")]
        public void CheckCanWeUpdateReinstateJobOnImport()
        {
            foreach (WellStatus item in System.Enum.GetValues(typeof(WellStatus)))
            {
                var sut = JobFactory.New
                    .With(p => p.WellStatus = item)
                    .Build();

                var testedValue = item != WellStatus.Complete;

                Assert.That(sut.CanWeUpdateJobOnImport(), Is.EqualTo(testedValue));
            }
        }

        public class TheIncludeJobTypeInImportMethod : JobExtensionsTests
        {
            [Test]
            public void ShouldNotIncludeOversInImport()
            {
                var job = JobFactory.New.With(p => p.InvoiceNumber = Job.OverInvoiceNumber).Build();

                Assert.IsFalse(job.IncludeJobTypeInImport());
            }

            [Test]
            public void ShouldIgnoreCetainJobTypes()
            {
                foreach (var jobType in Enum.GetValues(typeof(JobType)).Cast<JobType>())
                {
                    var job = JobFactory.New.With(p => p.JobTypeCode = EnumExtensions.GetDescription(jobType)).Build();
                    switch (job.JobType)
                    {
                        case JobType.Unknown:
                            Assert.IsFalse(job.IncludeJobTypeInImport());
                            break;
                        case JobType.Tobacco:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.Ambient:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.Alcohol:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.Chilled:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.Frozen:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.Documents:
                            Assert.IsFalse(job.IncludeJobTypeInImport());
                            break;
                        case JobType.SandwichUplift:
                            Assert.IsFalse(job.IncludeJobTypeInImport());
                            break;
                        case JobType.GlobalUplift:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.AssetsUplift:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.StandardUplift:
                            Assert.IsTrue(job.IncludeJobTypeInImport());
                            break;
                        case JobType.NotDefined:
                            Assert.IsFalse(job.IncludeJobTypeInImport());
                            break;
                        default:
                            Assert.Fail("Check that the new Job Type should be imported");
                            break;
                    }
                }
            }
        }


    }
}
