namespace PH.Well.UnitTests.Services.EpodServices
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Domain.ValueObjects;
    using Well.Services.Contracts;
    using Well.Services.EpodServices;


    [TestFixture]
    public class AdamFileImportCommandsTests
    {
        private Mock<IJobRepository> jobRepository;
        private Mock<IPostImportRepository> postImportRepository;
        private Mock<IStopRepository> stopRepository;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IAdamImportMapper> importMapper;
        private AdamFileImportCommands commands;

        [SetUp]
        public virtual void SetUp()
        {
            jobRepository = new Mock<IJobRepository>();
            postImportRepository = new Mock<IPostImportRepository>();
            stopRepository = new Mock<IStopRepository>();
            jobDetailRepository = new Mock<IJobDetailRepository>();
            importMapper = new Mock<IAdamImportMapper>();

            commands = new AdamFileImportCommands(
                jobRepository.Object,
                postImportRepository.Object,
                stopRepository.Object,
                jobDetailRepository.Object,
                importMapper.Object
            );
        }

        public class TheGetJobsToBeDeletedMethod : AdamFileImportCommandsTests
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
                    new JobStop{ JobId = 5, StopId = 99 }, // for deletion
                    new JobStop{ JobId = 6, StopId = 56 }, // for deletion
                };

                var completedStops = new List<Stop> { new Stop { Id = 57 } };

                jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(new List<Job>());

                commands.GetJobsToBeDeleted(existingRouteJobIdAndStopId, existingJobsBothSources, completedStops);

                jobRepository.Verify(x => x.GetByIds(It.Is<IEnumerable<int>>(jobIds =>
                    jobIds.Count() == 3
                    && jobIds.Contains(4)
                    && jobIds.Contains(5)
                    && jobIds.Contains(6)
                )), Times.Once);

            }
        }

    }
}