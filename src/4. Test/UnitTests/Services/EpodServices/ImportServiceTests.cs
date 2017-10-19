﻿namespace PH.Well.UnitTests.Services.EpodServices
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
    using Well.Domain.Contracts;

    [TestFixture]
    public class ImportServiceTests
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

        public class TheIsJobReplannedMethod : ImportServiceTests
        {
            [Test]
            [Ignore("I need to refactor this test")]
            public void ShouldReplanIfJobIsBypassedAndHasMovedStops()
            {
                //IList<StopImportStatus> stopImportStatuses = new List<StopImportStatus>();
                //Job fileJob = JobFactory.New.Build();
                //Job originalJob = JobFactory.New.Build();

                //mockImportService.Setup(x => x.HasJobMovedStops(originalJob, fileJob)).Returns(true);

                //foreach (WellStatus wellStatus in Enum.GetValues(typeof(WellStatus)))
                //{
                //    originalJob.WellStatus = wellStatus;
                //    var isReplanned = mockImportService.Object.IsJobReplanned(stopImportStatuses, fileJob, originalJob);

                //    if (wellStatus == WellStatus.Bypassed)
                //    {
                //        Assert.That(isReplanned, Is.True);
                //    }
                //    else
                //    {
                //        Assert.That(isReplanned, Is.False);
                //    }
                //}
            }

            [Test]
            [Ignore("I need to refactor this test")]
            public void ShouldReplanIfJobIsBypassedAndStopReplanned()
            {
                //var stop = StopFactory.New.Build();
                //var originalStopIdentifier = StopFactory.New.Build();

                //IList<StopImportStatus> stopImportStatuses = new List<StopImportStatus>
                //{
                //    new StopImportStatus(stop,StopImportStatus.Status.Updated,originalStopIdentifier)
                // };

                //Job fileJob = JobFactory.New.With(j => j.StopId = stop.Id).Build();
                //Job originalJob = JobFactory.New.Build();

                //mockImportService.Setup(x => x.HasJobMovedStops(originalJob, fileJob)).Returns(false);
                //mockImportService.Setup(x => x.HasStopBeenReplanned(stop, originalStopIdentifier)).Returns(true);

                //foreach (WellStatus wellStatus in Enum.GetValues(typeof(WellStatus)))
                //{
                //    originalJob.WellStatus = wellStatus;
                //    var isReplanned = mockImportService.Object.IsJobReplanned(stopImportStatuses, fileJob, originalJob);

                //    if (wellStatus == WellStatus.Bypassed)
                //    {
                //        Assert.That(isReplanned, Is.True);
                //    }
                //    else
                //    {
                //        Assert.That(isReplanned, Is.False);
                //    }
                //}
            }
        }

        public class TheHasJobMovedStopMethod : ImportServiceTests
        {
            [Test]
            [Ignore("I need to refactor this test")]
            public void ShouldReturnFalseIfStopIdsAreEqual()
            {
                //Job fileJob = JobFactory.New.With(j => j.StopId = 1).Build();
                //Job originalJob = JobFactory.New.With(j => j.StopId = 1).Build();
                //mockImportService.CallBase = true;
                //Assert.That(mockImportService.Object.HasJobMovedStops(originalJob, fileJob), Is.False);
            }

            [Test]
            [Ignore("I need to refactor this test")]
            public void ShouldReturnTrueIfStopIdsAreNotEqual()
            {
                //Job fileJob = JobFactory.New.With(j=> j.StopId = 1).Build();
                //Job originalJob = JobFactory.New.With(j => j.StopId = 2).Build();
                //mockImportService.CallBase = true;
                //Assert.That(mockImportService.Object.HasJobMovedStops(originalJob, fileJob), Is.True);
            }
        }

        public class TheHasStopBeenReplannedMethod : ImportServiceTests
        {
            [Test]
            public void StopIsReplannedIfItHasMovedToADifferntRoute()
            {
                IStopMoveIdentifiers newIdentifier = StopFactory.New.With(s => s.RouteHeaderId = 1).Build();
                IStopMoveIdentifiers original = StopFactory.New.With(s => s.RouteHeaderId = 2).Build();
                mockImportService.CallBase = true;
                Assert.That(mockImportService.Object.HasStopBeenReplanned(newIdentifier, original), Is.True);
            }

            //[Test]
            //public void StopIsReplannedIfPlannedStopNumberHasMovedForward()
            //{
            //    IStopMoveIdentifiers newIdentifier = StopFactory.New.With(s => s.PlannedStopNumber = "02").Build();
            //    IStopMoveIdentifiers original = StopFactory.New.With(s => s.PlannedStopNumber = "01").Build();
            //    mockImportService.CallBase = true;
            //    Assert.That(mockImportService.Object.HasStopBeenReplanned(newIdentifier, original), Is.True);
            //}

            [Test]
            public void StopIsNotReplannedIfPlannedStopNumberHasMovedBackwardsAndRouteRemainsTheSame()
            {
                IStopMoveIdentifiers newIdentifier = StopFactory.New.With(s => s.PlannedStopNumber = "01").Build();
                IStopMoveIdentifiers original = StopFactory.New.With(s => s.PlannedStopNumber = "02").Build();
                mockImportService.CallBase = true;
                Assert.That(mockImportService.Object.HasStopBeenReplanned(newIdentifier, original), Is.False);
            }

            [Test]
            public void StopIsNotReplannedIfNewPlannedStopNoIsNotANumber()
            {
                IStopMoveIdentifiers newIdentifier = StopFactory.New.With(s => s.PlannedStopNumber = "x").Build();
                IStopMoveIdentifiers original = StopFactory.New.With(s => s.PlannedStopNumber = "02").Build();
                mockImportService.CallBase = true;
                Assert.That(mockImportService.Object.HasStopBeenReplanned(newIdentifier, original), Is.False);
            }
            [Test]
            public void StopIsNotReplannedIfOriginalPlannedStopNoIsNotANumber()
            {
                IStopMoveIdentifiers newIdentifier = StopFactory.New.With(s => s.PlannedStopNumber = "01").Build();
                IStopMoveIdentifiers original = StopFactory.New.With(s => s.PlannedStopNumber = "x").Build();
                mockImportService.CallBase = true;
                Assert.That(mockImportService.Object.HasStopBeenReplanned(newIdentifier, original), Is.False);
            }
        }
    }
}