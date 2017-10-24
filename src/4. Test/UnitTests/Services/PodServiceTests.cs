namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class PodServiceTests
    {
        private Mock<IExceptionEventRepository> exceptionRepository;

        private Mock<IDateThresholdService> dateThresholdService;

        private PodService service;

        [SetUp]
        public void Setup()
        {
            this.exceptionRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            this.dateThresholdService = new Mock<IDateThresholdService>(MockBehavior.Strict);

            this.service = new PodService(this.exceptionRepository.Object, this.dateThresholdService.Object);
        }

        public class TheCreatePodEventMethod : PodServiceTests
        {
            [Test]
            public void ShouldCreatePodEvent()
            {
                var job = new Job {Id = 1, RoyaltyCode = "1234"};
                var branchId = 2;
                var date = DateTime.Now;

                this.exceptionRepository.Setup(x => x.IsPodEventCreatedForJob(It.IsAny<string>())).Returns(false);
                this.exceptionRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>(), It.IsAny<DateTime>()));
                this.dateThresholdService.Setup(
                    x => x.GracePeriodEnd(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(DateTime.Now);

                this.service.CreatePodEvent(job, branchId, date);

                this.exceptionRepository.Verify(x => x.IsPodEventCreatedForJob(It.IsAny<string>()), Times.Once);
                this.exceptionRepository.Verify(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
            }

            [Test]
            public void ShouldNotCreatePodEventBecauseItAlreadyExists()
            {
                var job = new Job { Id = 1, RoyaltyCode = "1234" };
                var branchId = 2;
                var date = DateTime.Now;

                this.exceptionRepository.Setup(x => x.IsPodEventCreatedForJob(It.IsAny<string>())).Returns(true);
                this.exceptionRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>(), It.IsAny<DateTime>()));
                this.dateThresholdService.Setup(
                    x => x.GracePeriodEnd(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(DateTime.Now);

                this.service.CreatePodEvent(job, branchId, date);

                this.exceptionRepository.Verify(x => x.IsPodEventCreatedForJob(It.IsAny<string>()), Times.Once);
                this.exceptionRepository.Verify(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
            }
        }
        
    }
}