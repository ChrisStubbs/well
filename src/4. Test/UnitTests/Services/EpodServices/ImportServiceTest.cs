namespace PH.Well.UnitTests.Services.EpodServices
{
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Services.Contracts;
    using Well.Services.EpodServices;

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
    }
}