using System;
using System.Collections;
using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Services;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Services
{
    [TestFixture]
    public class JobResolutionStatusTests
    {
        [Test]
        [Description("Check if the Job is in DriverCompleted status")]
        public void Test_ResolutionStatus_DriverCompleted()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();

            var sut = new JobResolutionStatus();
            var newStatus = sut.GetStatus(job);

            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.DriverCompleted));

            job.LineItems.Add(LineItemFactory.New.AddCloseAction().Build());

            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.DriverCompleted));
        }

        [Test]
        [Description("Check if the Job is in ActionRequired status")]
        public void Test_ResolutionStatus_ActionRequired()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();

            var sut = new JobResolutionStatus();
            var newStatus = sut.GetStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.ActionRequired));

            job.LineItems[0] = LineItemFactory.New.AddCloseAction().Build();
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.ActionRequired));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.ActionRequired));
        }

        [Test]
        [Description("Check if the Job is in PendingSubmission status")]
        public void Test_ResolutionStatus_PendingSubmission()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the status*/)
                .Build();

            var sut = new JobResolutionStatus();
            var newStatus = sut.GetStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .Build();
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.PendingSubmission));

            job.ResolutionStatus = ResolutionStatus.PendingApproval;
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));
        }

        [Test]
        [Description("Check if the Job is in PendingApproval status")]
        public void Test_ResolutionStatus_PendingApproval()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();

            var sut = new JobResolutionStatus();
            var newStatus = sut.GetStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingSubmission)
                .Build();
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job.ResolutionStatus = ResolutionStatus.PendingApproval;
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.PendingApproval));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));
        }

        [Test]
        [Description("Check if the Job is in Approved status")]
        public void Test_ResolutionStatus_Approved()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();

            var sut = new JobResolutionStatus();
            var newStatus = sut.GetStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job.ResolutionStatus = ResolutionStatus.Approved;
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Approved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));
        }

        [Test]
        [Description("Check if the Job is in Credited status")]
        public void Test_ResolutionStatus_Credited()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();

            var sut = new JobResolutionStatus();
            var newStatus = sut.GetStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job.ResolutionStatus = ResolutionStatus.Credited;
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();

            job.ResolutionStatus = ResolutionStatus.Credited;
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Credited));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));
        }
        
        [Test]
        [Description("Check if the Job is in Resolved status")]
        public void Test_ResolutionStatus_Resolved()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();

            var sut = new JobResolutionStatus();
            var newStatus = sut.GetStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            job.ResolutionStatus = ResolutionStatus.Resolved;
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Resolved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));
        }

        [Test]
        [Description("Check the progress over the ResolutionStatus")]
        [TestCaseSource(typeof(JobResolutionStatusTestsSource), nameof(JobResolutionStatusTestsSource.StepForward))]
        public ResolutionStatus StepForward(Job job)
        {
            var sut = new JobResolutionStatus();

            return sut.StepForward(job);
        }
    }

    class JobResolutionStatusTestsSource
    {
        public static IEnumerable StepForward
        {
            get
            {
                yield return new TestCaseData(GoodPendingSubmission()).Returns(ResolutionStatus.Approved).SetDescription("Job should move to Approved");
                yield return new TestCaseData(BadPendingSubmission()).Returns(ResolutionStatus.Invalid);
            }
        }

        private static Job GoodPendingSubmission()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddCreditAction().Build()))
                .Build();
        }

        private static Job BadPendingSubmission()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddNotDefinedAction().Build()))
                .Build();
        }
    }
}