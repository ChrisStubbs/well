namespace PH.Well.UnitTests.Services
{
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class BulkEditServiceTests
    {
        private Mock<ILogger> logger;
        private Mock<IJobRepository> jobRepository;
        private Mock<IBulkEditSummaryMapper> mapper;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IUserRepository> userRepository;
        private Mock<ILineItemActionRepository> lineItemActionRepository;
        private Mock<IJobService> jobService;
        private BulkEditService bulkEditService;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>();
            jobRepository = new Mock<IJobRepository>();
            mapper = new Mock<IBulkEditSummaryMapper>();
            userNameProvider = new Mock<IUserNameProvider>();
            userRepository = new Mock<IUserRepository>();
            lineItemActionRepository = new Mock<ILineItemActionRepository>();
            jobService = new Mock<IJobService>();

            bulkEditService = new BulkEditService(
                logger.Object,
                jobService.Object,
                jobRepository.Object,
                mapper.Object,
                userNameProvider.Object,
                userRepository.Object,
                lineItemActionRepository.Object
                );
        }

        public class TheGetEditableJobsMethod : BulkEditServiceTests
        {
            public void ShouldOnlyReturnJobsAssignedToUser()
            {
               //var jobs = new Li
            }
        }
    }
}