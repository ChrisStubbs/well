namespace PH.Well.UnitTests.Services.EpodServices
{
    using System.Collections.Generic;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Services.Contracts;
    using Well.Services.EpodServices;
    using System;

    [TestFixture]
    public class ImportServiceTest
    {
        private Mock<ILogger> logger;
        private Mock<IStopRepository> stopRepository;
        private Mock<IAccountRepository> accountRepository;
        private Mock<IJobRepository> jobRepository;
        private Mock<IJobService> jobService;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IJobDetailDamageRepository> jobDetailDamageRepository;
        private ImportService importService;
        private Mock<IStopService> stopService;
        private Mock<ImportService> mockImportService;

        [SetUp]
        public virtual void SetUp()
        {
            logger = new Mock<ILogger>();
            stopRepository = new Mock<IStopRepository>();
            accountRepository = new Mock<IAccountRepository>();
            jobRepository = new Mock<IJobRepository>();
            jobService = new Mock<IJobService>();
            jobDetailRepository = new Mock<IJobDetailRepository>();
            jobDetailDamageRepository = new Mock<IJobDetailDamageRepository>();
            stopService = new Mock<IStopService>();

            mockImportService = new Mock<ImportService>(
                logger.Object,
                stopRepository.Object,
                accountRepository.Object,
                jobRepository.Object,
                jobService.Object,
                jobDetailRepository.Object,
                jobDetailDamageRepository.Object,
                stopService.Object);

            importService = new ImportService(
                logger.Object,
                stopRepository.Object,
                accountRepository.Object,
                jobRepository.Object,
                jobService.Object,
                jobDetailRepository.Object,
                jobDetailDamageRepository.Object,
                stopService.Object);
        }

        public class TheIsJobReplannedMethod : ImportServiceTest
        {
            [Test]
            public void ShouldReplanIfJobIsBypassedAndHasMovedStops()
            {
                IList<StopImportStatus> stopImportStatuses = new List<StopImportStatus>();
                Job fileJob = JobFactory.New.Build();
                Job originalJob = JobFactory.New.Build();

                mockImportService.Setup(x => x.HasJobMovedStops(originalJob, fileJob)).Returns(true);

                foreach (WellStatus wellStatus in Enum.GetValues(typeof(WellStatus)))
                {
                    originalJob.WellStatus = wellStatus;
                    var isReplanned = mockImportService.Object.IsJobReplanned(stopImportStatuses, fileJob, originalJob);

                    if (wellStatus == WellStatus.Bypassed)
                    {
                        Assert.That(isReplanned, Is.True);
                    }
                    else
                    {
                        Assert.That(isReplanned, Is.False);
                    }
                }
            }

            [Test]
            public void ShouldReplanIfJobIsBypassedAndStopReplanned()
            {
                var stop = StopFactory.New.Build();
                var originalStopIdentifier = StopFactory.New.Build();

                IList<StopImportStatus> stopImportStatuses = new List<StopImportStatus>
                {
                    new StopImportStatus(stop,StopImportStatus.Status.Updated,originalStopIdentifier)
                 };

                Job fileJob = JobFactory.New.With(j => j.StopId = stop.Id).Build();
                Job originalJob = JobFactory.New.Build();

                mockImportService.Setup(x => x.HasJobMovedStops(originalJob, fileJob)).Returns(false);
                mockImportService.Setup(x => x.HasStopBeenReplanned(stop, originalStopIdentifier)).Returns(true);

                foreach (WellStatus wellStatus in Enum.GetValues(typeof(WellStatus)))
                {
                    originalJob.WellStatus = wellStatus;
                    var isReplanned = mockImportService.Object.IsJobReplanned(stopImportStatuses, fileJob, originalJob);

                    if (wellStatus == WellStatus.Bypassed)
                    {
                        Assert.That(isReplanned, Is.True);
                    }
                    else
                    {
                        Assert.That(isReplanned, Is.False);
                    }
                }
            }

        }

        public class TheHasJobMovedStopMethod : ImportServiceTest
        {
            [Test]
            public void ShouldReturnFalseIfStopIdsAreEqual()
            {
                Job fileJob = JobFactory.New.With(j => j.StopId = 1).Build();
                Job originalJob = JobFactory.New.With(j => j.StopId = 1).Build();
                mockImportService.CallBase = true;
                Assert.That(mockImportService.Object.HasJobMovedStops(originalJob, fileJob), Is.False);
            }

            [Test]
            public void ShouldReturnTrueIfStopIdsAreNotEqual()
            {
                Job fileJob = JobFactory.New.With(j=> j.StopId = 1).Build();
                Job originalJob = JobFactory.New.With(j => j.StopId = 2).Build();
                mockImportService.CallBase = true;
                Assert.That(mockImportService.Object.HasJobMovedStops(originalJob, fileJob), Is.True);
            }
        }
    }
}