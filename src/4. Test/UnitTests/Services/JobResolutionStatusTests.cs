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
        [Category("JobResolutionStatus")]
        public void Test_ResolutionStatus_DriverCompleted()
        {
            var job = JobFactory.New.Build();
            var detail = JobDetailFactory.New.Build();
            var wrongDetail = JobDetailFactory.New
                .With(p=> p.ShortQty = 10)
                .Build();

            var sut = new JobResolutionStatus();

            job.JobDetails.Add(detail);
            job.JobDetails.Add(detail);

            var newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.DriverCompleted));

            job.JobDetails.Add(wrongDetail);
            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.DriverCompleted));

            job = JobFactory.New.Build();
            wrongDetail = JobDetailFactory.New
                .With(p => p.JobDetailDamages.Add(new JobDetailDamage {  Qty = 10 }))
                .Build();

            job.JobDetails.Add(wrongDetail);

            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.DriverCompleted));

            job = JobFactory.New.Build();
            wrongDetail = JobDetailFactory.New
                .With(p => p.Actions.Add(new JobDetailAction { Quantity = 10 }))
                .Build();

            job.JobDetails.Add(wrongDetail);

            newStatus = sut.GetStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.DriverCompleted));
        }

        [Test]
        [Description("Check if the Job is in ActionRequired status")]
        [Category("JobResolutionStatus")]
        public void Test_ResolutionStatus_ActionRequired()
        {

        }
    }
}
