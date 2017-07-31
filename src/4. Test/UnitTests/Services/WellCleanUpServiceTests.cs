using System;
using System.Collections.Generic;
using tk = System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.UnitTests.Factories;
using PH.Well.Services.Contracts;
using PH.Well.Common.Contracts;

namespace PH.Well.UnitTests.Services
{
    [TestFixture]
    class WellCleanUpServiceTests
    {
        [Test]
        public void Should_Invoke_Royalty_GracePeriod()
        {
            var wellCleanUpRepository = new Mock<IWellCleanUpRepository>();
            var dateThresholdService = new Mock<IDateThresholdService>();
            var amendmentService = new Mock<IAmendmentService>();

            var nonSoftDeletedRoutesJobs = new List<NonSoftDeletedRoutesJobs>();

            nonSoftDeletedRoutesJobs.Add(NonSoftDeletedRoutesJobsFactory.New.With(p => p.JobRoyaltyCodeId = 1).With(p => p.JobRoyaltyCode = "JobRoyaltyCode").With(p => p.JobId = 188).Build());
            nonSoftDeletedRoutesJobs.Add(NonSoftDeletedRoutesJobsFactory.New.With(p => p.JobRoyaltyCodeId = 2).With(p => p.JobRoyaltyCode = "JobRoyaltyCode").With(p => p.RouteDate = DateTime.Now.AddDays(-10).Date).Build());

            wellCleanUpRepository.Setup(p => p.GetNonSoftDeletedRoutes()).Returns(nonSoftDeletedRoutesJobs);
            dateThresholdService.Setup(p => p.GracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>(), 1)).Returns(tk.Task.FromResult(DateTime.Now.AddDays(-1)));
            dateThresholdService.Setup(p => p.GracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>(), 2)).Returns(tk.Task.FromResult(DateTime.Now.AddDays(1)));

            var sut = this.Create(null, wellCleanUpRepository, dateThresholdService, amendmentService);

            sut.SoftDelete().Wait();
            dateThresholdService.Verify(p => p.GracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            amendmentService.Verify(p => p.ProcessAmendmentsAsync(new List<int> { nonSoftDeletedRoutesJobs[0].JobId }), Times.Once);
        }

        [Test]
        public void Should_Not_Invoke_Royalty_GracePeriod()
        {
            var wellCleanUpRepository = new Mock<IWellCleanUpRepository>();
            var dateThresholdService = new Mock<IDateThresholdService>();
            var nonSoftDeletedRoutesJobs = new List<NonSoftDeletedRoutesJobs> { NonSoftDeletedRoutesJobsFactory.New.Build() };

            wellCleanUpRepository.Setup(p => p.GetNonSoftDeletedRoutes()).Returns(nonSoftDeletedRoutesJobs);
            dateThresholdService.Setup(p => p.RouteGracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(tk.Task.FromResult(DateTime.Now.AddDays(-1)));

            var sut = this.Create(null, wellCleanUpRepository, dateThresholdService);

            sut.SoftDelete().Wait();
            dateThresholdService.Verify(p => p.RouteGracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once);
        }

        private WellCleanUpService Create(
            Mock<ILogger> logger = null,
            Mock<IWellCleanUpRepository> wellCleanUpRepository = null,
            Mock<IDateThresholdService> dateThresholdService = null,
            Mock<IAmendmentService> amendmentService = null,
            Mock<IJobRepository> jobRepository = null)
        {
            if (logger == null)
            {
                logger = new Mock<ILogger>();
            }
            if (wellCleanUpRepository == null)
            {
                wellCleanUpRepository = new Mock<IWellCleanUpRepository>();
            }
            if (dateThresholdService == null)
            {
                dateThresholdService = new Mock<IDateThresholdService>();
            }
            if (amendmentService == null)
            {
                amendmentService = new Mock<IAmendmentService>();
            }
            if (jobRepository == null)
            {
                jobRepository = new Mock<IJobRepository>();
            }

            return new WellCleanUpService(logger.Object, wellCleanUpRepository.Object, dateThresholdService.Object, amendmentService.Object, jobRepository.Object);
        }
    }
}
