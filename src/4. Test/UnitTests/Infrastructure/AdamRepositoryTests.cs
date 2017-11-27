namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using AIA.ADAM.DataProvider;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class AdamRepositoryTests
    {
        private Mock<ILogger> logger;
        private Mock<IJobRepository> jobRepository;
        private Mock<IDeliveryReadRepository> deliveryReadRepository;
        private Mock<IEventLogger> eventLogger;
        private Mock<IPodTransactionFactory> podTransactionFactory;
        private Mock<IExceptionEventRepository> eventRepository;
        private Mock<IGlobalUpliftTransactionFactory> globalUpliftTransactionFactory;


        private Mock<AdamRepository> moqAdamRepository;


        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger>();
            jobRepository = new Mock<IJobRepository>();
            deliveryReadRepository = new Mock<IDeliveryReadRepository>();
            eventLogger = new Mock<IEventLogger>();
            podTransactionFactory = new Mock<IPodTransactionFactory>();
            eventRepository = new Mock<IExceptionEventRepository>();
            globalUpliftTransactionFactory = new Mock<IGlobalUpliftTransactionFactory>();

            moqAdamRepository = new Mock<AdamRepository>(
                logger.Object,
                jobRepository.Object,
                eventLogger.Object,
                podTransactionFactory.Object,
                deliveryReadRepository.Object,
                eventRepository.Object,
                globalUpliftTransactionFactory.Object
                );
        }

        [Test]
        [TestCase(2, 3)]
        [TestCase(10, 11)]
        public void SuccessfulCreditWritesCorrectNumberOfLinesToAdam(int numberOfLines, int numberOfAdamExecutes)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            CreditTransaction creditTransaction = GetCreditTransaction(numberOfLines);
            var adamSettings = new AdamSettings();

            command.Setup(x => x.ExecuteNonQuery());

            var result = moqAdamRepository.Object.Credit(creditTransaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(numberOfAdamExecutes));
            Assert.That(creditTransaction.CanWriteHeader == true);
            Assert.That(creditTransaction.LineSql.Count == 0);
            Assert.That(result == AdamResponse.Success);
        }


        [Test]
        [TestCase(2, 3)]
        [TestCase(10, 11)]
        public void SuccessfulPodTransactionWritesCorrectNumberOfLinesToAdam(int numberOfLines, int numberOfAdamExecutes)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            PodTransaction podTransaction = GetPodTransaction(numberOfLines);
            var adamSettings = new AdamSettings();
            command.Setup(x => x.ExecuteNonQuery());

            var result = moqAdamRepository.Object.PodTransaction(podTransaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(numberOfAdamExecutes));
            Assert.That(podTransaction.CanWriteHeader == true);
            Assert.That(podTransaction.LineSql.Count == 0);
            Assert.That(result == AdamResponse.Success);

        }

        [Test]
        public void SuccessfulGrnWritesLineToAdam()
        {
            var adamSettings = new AdamSettings();
            var grnEvent = new GrnEvent();
            var delivery = new DeliveryDetail {GrnNumber = "123456", AccountCode = "56666"};

            deliveryReadRepository.Setup(x => x.GetDeliveryById(It.IsAny<int>(), It.IsAny<string>())).Returns(delivery);

            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            command.Setup(x => x.ExecuteNonQuery());

            var result = moqAdamRepository.Object.Grn(grnEvent, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Once);
            Assert.That(result == AdamResponse.Success);

        }

        [Test]
        [TestCase("")]
        [TestCase("     ")]
        public void GrnNoNumberDoesNotWriteLineToAdam(string grnNumber)
        {
            var adamSettings = new AdamSettings();
            var grnEvent = new GrnEvent();
            var delivery = new DeliveryDetail { GrnNumber = grnNumber, AccountCode = "56666" };

            deliveryReadRepository.Setup(x => x.GetDeliveryById(It.IsAny<int>(), It.IsAny<string>())).Returns(delivery);

            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            command.Setup(x => x.ExecuteNonQuery());

            var result = moqAdamRepository.Object.Grn(grnEvent, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Never);
            Assert.That(result == AdamResponse.Unknown);

        }

        [Test]
        [TestCase(2, 3)]
        [TestCase(10, 11)]
        public void SuccessfulAmendmentWritesCorrectNumberOfLinesToAdam(int numberOfLines, int numberOfAdamExecutes)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            AmendmentTransaction transaction = GetAmendmentTransaction(numberOfLines);
            var adamSettings = new AdamSettings();

            command.Setup(x => x.ExecuteNonQuery());

            var result = moqAdamRepository.Object.AmendmentTransaction(transaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(numberOfAdamExecutes));
            Assert.That(transaction.CanWriteHeader == true);
            Assert.That(transaction.LineSql.Count == 0);
            Assert.That(result == AdamResponse.Success);
        }

        [Test]
        public void SuccessfulDocRecirculationWritesLineToAdam()
        {
            var adamSettings = new AdamSettings();
            var transaction = new DocumentRecirculationTransaction();

            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            command.Setup(x => x.ExecuteNonQuery());

            var result = moqAdamRepository.Object.DocumentRecirculation(transaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Once);
            Assert.That(result == AdamResponse.Success);

        }

        private CreditTransaction GetCreditTransaction(int numberOfLines)
        {
            return new CreditTransaction { HeaderSql = GetHeaderForTransaction(), JobId = 1, BranchId = 2, LineSql = GetLinesForTransaction(numberOfLines) };
        }

        private PodTransaction GetPodTransaction(int numberOfLines)
        {
            return new PodTransaction { HeaderSql = GetHeaderForTransaction(), JobId = 1, BranchId = 2, LineSql = GetLinesForTransaction(numberOfLines)};
        }

        private AmendmentTransaction GetAmendmentTransaction(int numberOfLines)
        {
            return new AmendmentTransaction { HeaderSql = GetHeaderForTransaction(), BranchId = 2, LineSql = GetLinesForTransaction(numberOfLines)};
        }

        private string GetHeaderForTransaction()
        {
            return "insert foo into header";
        }

        private Dictionary<int, string> GetLinesForTransaction(int numberOfLines)
        {
            var line = "insert bar in to line ";
            var lines = new Dictionary<int, string>();

            for (int i = 1; i <= numberOfLines; i++)
            {
                lines.Add(i, line + i);
            }

            return lines;
        }
    }
}