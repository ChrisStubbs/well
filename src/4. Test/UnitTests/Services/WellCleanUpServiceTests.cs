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

namespace PH.Well.UnitTests.Services
{
    using System.Configuration;
    using System.Linq;
    using NUnit.Framework.Internal;
    using ILogger = Well.Common.Contracts.ILogger;

    [TestFixture]
    public class WellCleanUpServiceTests
    {
        private Mock<ILogger> logger;
        private Mock<IWellCleanUpRepository> wellCleanUpRepository;
        private Mock<IDateThresholdService> dateThresholdService;
        private Mock<IAmendmentService> amendmentService;
        private Mock<IJobRepository> jobRepository;
        private Mock<IWellCleanConfig> config;
        private WellCleanUpService cleanupService;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger>();
            wellCleanUpRepository = new Mock<IWellCleanUpRepository>();
            dateThresholdService = new Mock<IDateThresholdService>();
            amendmentService = new Mock<IAmendmentService>();
            jobRepository = new Mock<IJobRepository>();
            config = new Mock<IWellCleanConfig>();
            config.SetupGet(x => x.SoftDeleteBatchSize).Returns(5000);

            cleanupService = new WellCleanUpService(
                logger.Object,
                wellCleanUpRepository.Object,
                dateThresholdService.Object,
                amendmentService.Object,
                jobRepository.Object,
                config.Object);
        }

        [Test]
        public void Should_Invoke_Royalty_GracePeriod()
        {
            var nonSoftDeletedRoutesJobs = new List<NonSoftDeletedRoutesJobs>();

            nonSoftDeletedRoutesJobs.Add(NonSoftDeletedRoutesJobsFactory.New.With(p => p.JobRoyaltyCodeId = 1).With(p => p.JobRoyaltyCode = "JobRoyaltyCode").With(p => p.JobId = 188).Build());
            nonSoftDeletedRoutesJobs.Add(NonSoftDeletedRoutesJobsFactory.New.With(p => p.JobRoyaltyCodeId = 2).With(p => p.JobRoyaltyCode = "JobRoyaltyCode").With(p => p.RouteDate = DateTime.Now.AddDays(-10).Date).Build());

            wellCleanUpRepository.Setup(p => p.GetNonSoftDeletedRoutes()).Returns(nonSoftDeletedRoutesJobs);
            dateThresholdService.Setup(p => p.GracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>(), 1)).Returns(tk.Task.FromResult(DateTime.Now.AddDays(-1)));
            dateThresholdService.Setup(p => p.GracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>(), 2)).Returns(tk.Task.FromResult(DateTime.Now.AddDays(1)));

            cleanupService.SoftDelete().Wait();

            dateThresholdService.Verify(p => p.GracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            amendmentService.Verify(p => p.ProcessAmendmentsAsync(new List<int> { nonSoftDeletedRoutesJobs[0].JobId }), Times.Once);
        }

        [Test]
        public void Should_Not_Invoke_Royalty_GracePeriod()
        {
            var nonSoftDeletedRoutesJobs = new List<NonSoftDeletedRoutesJobs> { NonSoftDeletedRoutesJobsFactory.New.Build() };

            wellCleanUpRepository.Setup(p => p.GetNonSoftDeletedRoutes()).Returns(nonSoftDeletedRoutesJobs);
            dateThresholdService.Setup(p => p.RouteGracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(tk.Task.FromResult(DateTime.Now.AddDays(-1)));

            cleanupService.SoftDelete().Wait();
            dateThresholdService.Verify(p => p.RouteGracePeriodEndAsync(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once);
        }

        public class TheSoftDeleteInBatchesMethod : WellCleanUpServiceTests
        {
            [Test]
            public void ShouldDeleteJobsInBatches()
            {
                jobRepository.Setup(x => x.JobsSetResolutionStatusClosed(It.IsAny<List<int>>()));
                jobRepository.Setup(x => x.CascadeSoftDeleteJobs(It.IsAny<List<int>>(), false));
                wellCleanUpRepository.Setup(x => x.DeleteStops(It.IsAny<List<int>>()));
                wellCleanUpRepository.Setup(x => x.DeleteRoutes(It.IsAny<List<int>>()));

                var jobIds = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                cleanupService.SoftDeleteInBatches(jobIds, 3);
                jobRepository.Verify(x => x.JobsSetResolutionStatusClosed(It.IsAny<List<int>>()), Times.Exactly(3));
                jobRepository.Verify(x => x.JobsSetResolutionStatusClosed(It.Is<List<int>>(ids => ids.All(new[] { 1, 2, 3 }.Contains))), Times.Once);
                jobRepository.Verify(x => x.JobsSetResolutionStatusClosed(It.Is<List<int>>(ids => ids.All(new[] { 4, 5, 6 }.Contains))), Times.Once);
                jobRepository.Verify(x => x.JobsSetResolutionStatusClosed(It.Is<List<int>>(ids => ids.All(new[] { 7, 8 }.Contains))), Times.Once);

                jobRepository.Verify(x => x.CascadeSoftDeleteJobs(It.IsAny<List<int>>(), false), Times.Exactly(3));
                jobRepository.Verify(x => x.CascadeSoftDeleteJobs(It.Is<List<int>>(ids => ids.All(new[] { 1, 2, 3 }.Contains)), false), Times.Once);
                jobRepository.Verify(x => x.CascadeSoftDeleteJobs(It.Is<List<int>>(ids => ids.All(new[] { 4, 5, 6 }.Contains)), false), Times.Once);
                jobRepository.Verify(x => x.CascadeSoftDeleteJobs(It.Is<List<int>>(ids => ids.All(new[] { 7, 8 }.Contains)), false), Times.Once);

                wellCleanUpRepository.Verify(x => x.DeleteStops(It.IsAny<List<int>>()), Times.Exactly(3));
                wellCleanUpRepository.Verify(x => x.DeleteStops(It.Is<List<int>>(ids => ids.All(new[] { 1, 2, 3 }.Contains))), Times.Once);
                wellCleanUpRepository.Verify(x => x.DeleteStops(It.Is<List<int>>(ids => ids.All(new[] { 4, 5, 6 }.Contains))), Times.Once);
                wellCleanUpRepository.Verify(x => x.DeleteStops(It.Is<List<int>>(ids => ids.All(new[] { 7, 8 }.Contains))), Times.Once);


                wellCleanUpRepository.Verify(x => x.DeleteRoutes(It.IsAny<List<int>>()), Times.Exactly(3));
                wellCleanUpRepository.Verify(x => x.DeleteRoutes(It.Is<List<int>>(ids => ids.All(new[] { 1, 2, 3 }.Contains))), Times.Once);
                wellCleanUpRepository.Verify(x => x.DeleteRoutes(It.Is<List<int>>(ids => ids.All(new[] { 4, 5, 6 }.Contains))), Times.Once);
                wellCleanUpRepository.Verify(x => x.DeleteRoutes(It.Is<List<int>>(ids => ids.All(new[] { 7, 8 }.Contains))), Times.Once);

            }
        }

    }
}

