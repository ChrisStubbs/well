using System;
using System.Collections;
using Moq;
using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Services.Contracts;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Services
{
    [TestFixture]
    public class JobResolutionStatusTests
    {
        private JobService sut;

        [SetUp]
        public void testSetup()
        {
            var userThreshold = new Mock<IUserThresholdService>();
            var dateThresholdService = new Mock<IDateThresholdService>();
            var jobRepository = new Mock<IJobRepository>();
            var assigneeReadRepository = new Mock<IAssigneeReadRepository>();
            var lineItemRepository = new Mock<ILineItemSearchReadRepository>();
            this.sut = new JobService(jobRepository.Object, userThreshold.Object, dateThresholdService.Object, assigneeReadRepository.Object, lineItemRepository.Object);
        }

        [Test]
        [Description("Check if the Job is in DriverCompleted status")]
        [Category("JobResolutionStatus get status")]
        [Category("JobService")]
        public void Test_ResolutionStatus_DriverCompleted()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();

            
            var newStatus = sut.GetCurrentResolutionStatus(job);

            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.DriverCompleted));

            job.LineItems.Add(LineItemFactory.New.AddCloseAction().Build());

            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.DriverCompleted));
        }

        [Test]
        [Description("Check if the Job is in ActionRequired status")]
        [Category("JobResolutionStatus get status")]
        [Category("JobService")]
        public void Test_ResolutionStatus_ActionRequired()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.ActionRequired));

            job.LineItems[0] = LineItemFactory.New.AddCloseAction().Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.ActionRequired));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.ActionRequired));
        }

        [Test]
        [Description("Check if the Job is in PendingSubmission status")]
        [Category("JobResolutionStatus get status")]
        [Category("JobService")]
        public void Test_ResolutionStatus_PendingSubmission()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the status*/)
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.PendingSubmission));

            job.ResolutionStatus = ResolutionStatus.PendingApproval;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));
        }

        [Test]
        public void ShouldReturnPendingSubmissionIfAlreadyPendingSubmission()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the status*/)
                .Build();
            job.ResolutionStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(job.ResolutionStatus, Is.EqualTo(ResolutionStatus.PendingSubmission));
            Assert.That(sut.GetCurrentResolutionStatus(job), Is.EqualTo(ResolutionStatus.PendingSubmission));
        }

        [Test]
        public void ShouldReturnPendingSubmissionIfCompletedByWell()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the status*/)
                .Build();
            job.ResolutionStatus = ResolutionStatus.CompletedByWell;
            Assert.That(job.ResolutionStatus, Is.EqualTo(ResolutionStatus.CompletedByWell));
            Assert.That(sut.GetCurrentResolutionStatus(job), Is.EqualTo(ResolutionStatus.PendingSubmission));
        }

        [Test]
        [Description("Check if the Job is in PendingApproval status")]
        [Category("JobResolutionStatus get status")]
        [Category("JobService")]
        public void Test_ResolutionStatus_PendingApproval()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingSubmission)
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job.ResolutionStatus = ResolutionStatus.PendingApproval;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.PendingApproval));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));
        }

        [Test]
        [Description("Check if the Job is in Approved status")]
        [Category("JobResolutionStatus get status")]
        [Category("JobService")]
        public void Test_ResolutionStatus_Approved()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job.ResolutionStatus = ResolutionStatus.Approved;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Approved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));
        }

        [Test]
        [Description("Check if the Job is in Credited status")]
        [Category("JobResolutionStatus get status")]
        [Category("JobService")]
        public void Test_ResolutionStatus_Credited()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job.ResolutionStatus = ResolutionStatus.Credited;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();

            job.ResolutionStatus = ResolutionStatus.Credited;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Credited));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));
        }
        
        [Test]
        [Description("Check if the Job is in Resolved status")]
        [Category("JobResolutionStatus get status")]
        [Category("JobService")]
        public void Test_ResolutionStatus_Resolved()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            job.ResolutionStatus = ResolutionStatus.Resolved;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Resolved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));
        }

    }

    [TestFixture]
    public class JobResolutionStatusStepForwardTests
    {
        [Test]
        [TestCaseSource(typeof(JobResolutionStatusTestsSource), nameof(JobResolutionStatusTestsSource.StepForward))]
        [Category("JobResolutionStatus StepForward")]
        [Category("JobService")]
        public ResolutionStatus JobResolutionStatusStepForward(Job job, IUserThresholdService userThresholdService, IDateThresholdService dateThresholdService)
        {
            var jobRepository = new Mock<IJobRepository>();
            var assigneeReadRepository = new Mock<IAssigneeReadRepository>();
            var lineItemRepository = new Mock<ILineItemSearchReadRepository>();
            var sut = new JobService(jobRepository.Object, userThresholdService, dateThresholdService, assigneeReadRepository.Object, lineItemRepository.Object);

            return sut.GetNextResolutionStatus(job);
        }
    }

    class JobResolutionStatusTestsSource
    {
        public static IEnumerable StepForward
        {
            get
            {
                yield return new TestCaseData(Imported(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.DriverCompleted)
                    .SetDescription("Job should move to DriverCompleted");

                yield return new TestCaseData(DriverCompletedOutWindowComplainNoActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should move to Close");

                yield return new TestCaseData(DriverCompletedOutWindowComplainWithActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("DriverCompleted Job should move to ActionRequired");

                yield return new TestCaseData(DriverCompletedInWindowComplainWithActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("DriverCompleted Job should move to ActionRequired");

                yield return new TestCaseData(DriverCompletedInWindowComplainNoActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should stay in DriverCompleted");

                yield return new TestCaseData(ActionRequired(true), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("ActionRequired Job should stay in ActionRequired");

                yield return new TestCaseData(ActionRequired(false), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.PendingSubmission)
                    .SetDescription("ActionRequired Job should move to PendingSubmission");

                yield return new TestCaseData(GoodPendingSubmission(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Approved)
                    .SetDescription("PendingSubmission Job should move to Approved");

                yield return new TestCaseData(BadPendingSubmission(), CreateUserThresholdService(false), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.PendingApproval)
                    .SetDescription("PendingApproval Job should not move to Approved");

                yield return new TestCaseData(Approved(true), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Credited)
                    .SetDescription("Approved Job should not move to Credited");

                yield return new TestCaseData(Approved(false), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Resolved)
                    .SetDescription("Approved Job should not move to Resolved");

                yield return new TestCaseData(CreditedOutWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.Credited)
                    .SetDescription("Credited Job should move  to Closed - Credited");

                yield return new TestCaseData(CreditedInWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.Credited)
                    .SetDescription("Credited Job should stay in Credited");

                yield return new TestCaseData(ResolvedOutWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.Resolved)
                    .SetDescription("Credited Job should move to Closed - Resolved");

                yield return new TestCaseData(ResolvedInWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.Resolved)
                    .SetDescription("Credited Job should stay in Resolved");
            }
        }
        
        private static IUserThresholdService CreateUserThresholdService(bool withinThreshol)
        {
            var mock = new Mock<IUserThresholdService>();

            mock.Setup(p => p.UserHasRequiredCreditThreshold(It.IsAny<Job>())).Returns(withinThreshol);

            return mock.Object;
        }

        private static IDateThresholdService DateThresholdService(DateTime date)
        {
            var mock = new Mock<IDateThresholdService>();

            mock.Setup(p => p.EarliestSubmitDate(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(date);
            
            return mock.Object;
        }

        private static Job DriverCompletedOutWindowComplainNoActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedOutWindowComplainWithActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedInWindowComplainWithActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedInWindowComplainNoActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job ActionRequired(bool addNotDefinedAction)
        {
            var job = JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.ResolutionStatus = ResolutionStatus.ActionRequired)
                .Build();

            if (addNotDefinedAction)
            {
                job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            }
            else
            {
                job.LineItems.Add(LineItemFactory.New.AddCreditAction().Build());
            }

            return job;
        }

        private static Job Imported()
        {
            return JobFactory.New
                .With(p => p.ResolutionStatus = ResolutionStatus.Imported)
                .Build();
        }

        private static Job GoodPendingSubmission()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingSubmission)
                .Build();
        }

        private static Job BadPendingSubmission()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddNotDefinedAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingSubmission)
                .Build();
        }

        private static Job Approved(bool withCredits)
        {
            var job = JobFactory.New
                .With(p => p.ResolutionStatus = ResolutionStatus.Approved)
                .Build();

            if (withCredits)
            {
                job.LineItems.Add(LineItemFactory.New.AddCreditAction().Build());
            }
            else
            {
                job.LineItems.Add(LineItemFactory.New.AddCloseAction().Build());
            }

            return job;
        }

        private static Job CreditedOutWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Credited)
                .Build();
        }

        private static Job CreditedInWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Credited)
                .Build();
        }

        private static Job ResolvedOutWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Resolved)
                .Build();
        }

        private static Job ResolvedInWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Resolved)
                .Build();
        }
    }
}