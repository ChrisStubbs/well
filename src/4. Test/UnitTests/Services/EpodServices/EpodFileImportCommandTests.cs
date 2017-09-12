namespace PH.Well.UnitTests.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using NUnit.Framework.Internal.Commands;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services;
    using Well.Services.Contracts;
    using Well.Services.EpodServices;

    [TestFixture]
    public class EpodFileImportCommandTests
    {
        private Mock<ILogger> logger;
        private Mock<IJobRepository> jobRepository;
        private Mock<IEpodImportMapper> epodImportMapper;
        private Mock<IJobService> jobService;
        private Mock<IExceptionEventRepository> exceptionEventRepository;
        private Mock<IDateThresholdService> dateThresholdService;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IJobDetailDamageRepository> jobDetailDamageRepository;
        private Mock<IPostImportRepository> postImportRepository;

        private EpodFileImportCommands commands;
        private Mock<EpodFileImportCommands> mockCommands;

        [SetUp]
        public virtual void SetUp()
        {
            logger = new Mock<ILogger>();
            jobRepository = new Mock<IJobRepository>();
            epodImportMapper = new Mock<IEpodImportMapper>();
            jobService = new Mock<IJobService>();
            exceptionEventRepository = new Mock<IExceptionEventRepository>();
            dateThresholdService = new Mock<IDateThresholdService>();
            jobDetailRepository = new Mock<IJobDetailRepository>();
            jobDetailDamageRepository = new Mock<IJobDetailDamageRepository>();
            postImportRepository = new Mock<IPostImportRepository>();


            commands = new EpodFileImportCommands(
                logger.Object,
                jobRepository.Object,
                epodImportMapper.Object,
                jobService.Object,
                exceptionEventRepository.Object,
                dateThresholdService.Object,
                jobDetailRepository.Object,
                jobDetailDamageRepository.Object,
                postImportRepository.Object);

            mockCommands = new Mock<EpodFileImportCommands>(
                logger.Object,
                jobRepository.Object,
                epodImportMapper.Object,
                jobService.Object,
                exceptionEventRepository.Object,
                dateThresholdService.Object,
                jobDetailRepository.Object,
                jobDetailDamageRepository.Object,
                postImportRepository.Object)
            { CallBase = true };
        }

        public class AfterJobCreationMethod : EpodFileImportCommandTests
        {
            [Test]
            public void ShouldCallUpdateExistingJobsWithEvents()
            {
                exceptionEventRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>()));
                var fileJob = JobFactory.New.Build();
                var existingJob = JobFactory.New.With(x => x.ResolutionStatus = ResolutionStatus.Imported).Build();
                var routeHeader = RouteHeaderFactory.New.Build();

                mockCommands.Setup(x => x.UpdateExistingJob(It.IsAny<Job>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>()));

                mockCommands.Object.AfterJobCreation(fileJob, existingJob, routeHeader);
                mockCommands.Verify(x => x.UpdateExistingJob(It.IsAny<Job>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Times.Once);
                mockCommands.Verify(x => x.UpdateExistingJob(fileJob, existingJob, routeHeader.RouteOwnerId, routeHeader.RouteDate.Value, true), Times.Once);

                Assert.That(existingJob.ResolutionStatus, Is.EqualTo(ResolutionStatus.DriverCompleted));
            }
        }

        public class TheUpdateWithouEventsMethod : EpodFileImportCommandTests
        {
            [Test]
            public void ShouldCallUpdateExistingJobsWithoutEvents()
            {
                exceptionEventRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>()));
                var existingJob = JobFactory.New.With(x => x.ResolutionStatus = ResolutionStatus.Imported).Build();
                var branchId = 22;
                var routeDate = DateTime.Now;

                mockCommands.Setup(x => x.UpdateExistingJob(It.IsAny<Job>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>()));

                mockCommands.Object.UpdateWithoutEvents(existingJob, branchId, routeDate);
                mockCommands.Verify(x => x.UpdateExistingJob(It.IsAny<Job>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Times.Once);
                mockCommands.Verify(x => x.UpdateExistingJob(existingJob, existingJob, branchId, routeDate, false), Times.Once);

                Assert.That(existingJob.ResolutionStatus, Is.EqualTo(ResolutionStatus.Imported));
            }

        }

        public class TheUpdateExistingJobMethod : EpodFileImportCommandTests
        {
            [Test]
            public void ShouldCallUpdateExistingJobsWithEvents()
            {
                exceptionEventRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>()));
                var fileJob = JobFactory.New.Build();
                var existingJob = JobFactory.New.With(x => x.ResolutionStatus = ResolutionStatus.Imported).Build();
                var routeHeader = RouteHeaderFactory.New.Build();

                mockCommands.Setup(x => x.UpdateExistingJob(It.IsAny<Job>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>()));

                mockCommands.Object.UpdateExistingJob(fileJob, existingJob, routeHeader);
                mockCommands.Verify(x => x.UpdateExistingJob(It.IsAny<Job>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Times.Once);
                mockCommands.Verify(x => x.UpdateExistingJob(fileJob, existingJob, routeHeader.RouteOwnerId, routeHeader.RouteDate.Value, true), Times.Once);

                Assert.That(existingJob.ResolutionStatus, Is.EqualTo(ResolutionStatus.DriverCompleted));
            }

            [Test]
            public void ShouldNotInsertEventForCompletedOnPaperPod()
            {
                exceptionEventRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>()));
                var fileJob = JobFactory.New.Build();
                var existingJob = JobFactory.New
                                    .With(x => x.ProofOfDelivery = (int)ProofOfDelivery.CocaCola)
                                    .With(x => x.JobStatus = JobStatus.CompletedOnPaper)
                                    .Build();

                var routeHeader = RouteHeaderFactory.New.Build();

                commands.UpdateExistingJob(fileJob, existingJob, routeHeader);
                exceptionEventRepository.Verify(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>()), Times.Never);
            }

            [Test]
            public void ShouldInsertEventForPod()
            {
                exceptionEventRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>()));
                var fileJob = JobFactory.New.Build();
                var existingJob = JobFactory.New
                    .With(x => x.ProofOfDelivery = (int)ProofOfDelivery.CocaCola)
                    .Build();

                var routeHeader = RouteHeaderFactory.New.Build();

                commands.UpdateExistingJob(fileJob, existingJob, routeHeader);
                exceptionEventRepository.Verify(x => x.InsertPodEvent(It.Is<PodEvent>(
                    pod => pod.Id == existingJob.Id && pod.BranchId == routeHeader.RouteOwnerId
                ), It.IsAny<string>()), Times.Once);
                exceptionEventRepository.Verify(x => x.InsertPodEvent(It.IsAny<PodEvent>(), It.IsAny<string>()), Times.Once);
            }
        }

        public class ThePostJobImportMethod : EpodFileImportCommandTests
        {
            [Test]
            public void ShouldCallPostImortRepositoryAndRunPostInvoiceProcesing()
            {
                var jobIds = new List<int> { 1, 2 };
                postImportRepository.Setup(x => x.PostImportUpdate(jobIds));
                mockCommands.Setup(x => x.RunPostInvoicedProcessing(jobIds)).Returns(new List<Job>());
                //ACT
                mockCommands.Object.PostJobImport(jobIds);
                //ASSERT
                postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IList<int>>()), Times.Once);
                postImportRepository.Verify(x => x.PostImportUpdate(jobIds), Times.Once);
                mockCommands.Verify(x => x.RunPostInvoicedProcessing(jobIds), Times.Once);
            }
        }

        public class TheRunPostInvoicedProcessingMethod : EpodFileImportCommandTests
        {
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                postImportRepository.Setup(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()));
                postImportRepository.Setup(x => x.PostTranSendImport(It.IsAny<List<int>>()));
                postImportRepository.Setup(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()));
                jobRepository.Setup(x => x.GetJobsWithLineItemActions(It.IsAny<List<int>>())).Returns(It.IsAny<IEnumerable<int>>());
                jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(It.IsAny<IEnumerable<Job>>()); ;
                jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>())).Returns(new List<Job> { new Job() });
                jobService.Setup(x => x.GetNextResolutionStatus(It.IsAny<Job>())).Returns(ResolutionStatus.Imported);
                jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
                jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));
            }

            [Test]
            public void ShouldNotRunPostTransendImportIfNoJobIds()
            {

                var updatedJobIds = new List<int>();
                commands.RunPostInvoicedProcessing(updatedJobIds);
                postImportRepository.Verify(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()), Times.Never);
                postImportRepository.Verify(x => x.PostTranSendImport(It.IsAny<List<int>>()), Times.Never);
                postImportRepository.Verify(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()), Times.Never);
                jobRepository.Verify(x => x.GetJobsWithLineItemActions(It.IsAny<List<int>>()), Times.Never);
                jobRepository.Verify(x => x.GetByIds(It.IsAny<IEnumerable<int>>()), Times.Never);
                jobService.Verify(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>()), Times.Never);
                jobService.Verify(x => x.GetNextResolutionStatus(It.IsAny<Job>()), Times.Never);
                jobRepository.Verify(x => x.Update(It.IsAny<Job>()), Times.Never);
                jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            }

            [Test]
            public void ShouldRunPostTransendImportIfMoreThanOneJobId()
            {
                var updatedJobIds = new List<int> { 1, 2 };

                postImportRepository.Setup(x => x.PostTranSendImportForTobacco(updatedJobIds));
                postImportRepository.Setup(x => x.PostTranSendImport(updatedJobIds));
                postImportRepository.Setup(x => x.PostTranSendImportShortsTba(updatedJobIds));

                commands.RunPostInvoicedProcessing(updatedJobIds);

                postImportRepository.Verify(x => x.PostTranSendImportForTobacco(updatedJobIds), Times.Once);
                postImportRepository.Verify(x => x.PostTranSendImport(updatedJobIds), Times.Once);
                postImportRepository.Verify(x => x.PostTranSendImportShortsTba(updatedJobIds), Times.Once);
            }


            [Test]
            public void ShouldUpdateAndSaveJobStatusOnceForEachJob()
            {
                var updatedJobIds = new List<int> { 1, 2 };
                var job1 = JobFactory.New.With(x => x.Id = 1).Build();
                var job2 = JobFactory.New.With(x => x.Id = 2).Build();

                jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>())).Returns(new List<Job> { job1, job2 });

                jobService.Setup(x => x.GetNextResolutionStatus(job1)).Returns(ResolutionStatus.Imported);
                jobService.Setup(x => x.GetNextResolutionStatus(job2)).Returns(ResolutionStatus.Imported);
                jobRepository.Setup(x => x.Update(job1));
                jobRepository.Setup(x => x.Update(job2));

                jobRepository.Setup(x => x.SetJobResolutionStatus(1, ResolutionStatus.Imported.Description));
                jobRepository.Setup(x => x.SetJobResolutionStatus(2, ResolutionStatus.Imported.Description));

                commands.RunPostInvoicedProcessing(updatedJobIds);

                jobService.Verify(x => x.GetNextResolutionStatus(It.IsAny<Job>()), Times.Exactly(2));
                jobRepository.Verify(x => x.Update(It.IsAny<Job>()), Times.Exactly(2));
                jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));

            }
        }

        public class TheGetJobsToBeDeletedMethod : EpodFileImportCommandTests
        {
            [Test]
            public void ShouldOnlyGetJobsThatAreInCurrentStopsAndDontExistInBothSources()
            {
                var existingJobsBothSources = new List<Job>
                {
                    new Job{ Id = 1, StopId = 55 },
                    new Job{ Id = 2, StopId = 56 },
                    new Job{ Id = 3, StopId = 55 }
                };

                var existingRouteJobIdAndStopId = new List<JobStop>
                {
                    new JobStop{ JobId = 1, StopId = 55 }, // in both sources don't delete
                    new JobStop{ JobId = 2, StopId = 56 }, // in both sources don't delete
                    new JobStop{ JobId = 3, StopId = 55 }, // in both sources don't delete
                    new JobStop{ JobId = 4, StopId = 55 }, // for deletion
                    new JobStop{ JobId = 5, StopId = 99 }, // different stop so don't delete
                    new JobStop{ JobId = 6, StopId = 56 }, // for deletion
                };

                jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(new List<Job>());

                commands.GetJobsToBeDeleted(existingRouteJobIdAndStopId, existingJobsBothSources,new List<Stop>());

                jobRepository.Verify(x=> x.GetByIds(It.Is<IEnumerable<int>>( jobIds=>
                    jobIds.Count() == 2 
                    && jobIds.Contains(4)
                    && jobIds.Contains(6)
                    )),Times.Once);

            }
        }
    }

}

