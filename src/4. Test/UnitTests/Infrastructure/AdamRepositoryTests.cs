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
        [TestCase(8, 1, 8, false)]
        [TestCase(0, 1, 0, true)] // 0 line credit, fails on the header
        public void AdamExceptionOnFirstWriteSavesCompleteTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();

            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);
            eventRepository.Setup(x => x.InsertCreditEventTransaction(It.IsAny<CreditTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            command.Setup(m => m.ExecuteNonQuery()).Throws(new Exception("oops damn!"));

            CreditTransaction creditTransaction = GetCreditTransaction(numberOfLines);
            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.Credit(creditTransaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(creditTransaction.CanWriteHeader == canWriteHeader);
            Assert.That(creditTransaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertCreditEventTransaction(It.IsAny<CreditTransaction>()), Times.Never);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            Assert.That(result == AdamResponse.AdamDownNoChange);

        }

        [Test]
        [TestCase(8, 4, 5, false, 3)] // 8 line credit, fails on the 4th line
        [TestCase(4, 5, 0, true, 4)]  // 4 line credit, fails on header
        [TestCase(10, 10, 1, false, 9)] // 10 line credit, fails on the 10th line
        public void AdamExceptionNotOnFirstWriteSavesRemainingTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader, int errorTry)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();

            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            eventRepository.Setup(x => x.InsertCreditEventTransaction(It.IsAny<CreditTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var g = 1;
            command.Setup(m => m.ExecuteNonQuery()).Callback(() =>
            {
                if (g == errorTry)
                {
                    command.Setup(n => n.ExecuteNonQuery()).Throws(new Exception("seriously?"));
                }
                else
                {
                    g++;
                }
            });

            CreditTransaction creditTransaction = GetCreditTransaction(numberOfLines);
            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.Credit(creditTransaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(creditTransaction.CanWriteHeader == canWriteHeader);
            Assert.That(creditTransaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertCreditEventTransaction(It.IsAny<CreditTransaction>()), Times.Once);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);

            Assert.That(result == AdamResponse.AdamDown);

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
        [TestCase(8, 1, 8, false)]
        [TestCase(0, 1, 0, true)] // 0 line credit, fails on the header
        public void PodTransactionAdamExceptionOnFirstWriteSavesCompletePodTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();

            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);
            eventRepository.Setup(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            command.Setup(m => m.ExecuteNonQuery()).Throws(new Exception("oops damn!"));

            PodTransaction podTransaction = GetPodTransaction(numberOfLines);
            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.PodTransaction(podTransaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(podTransaction.CanWriteHeader == canWriteHeader);
            Assert.That(podTransaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()), Times.Never);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            Assert.That(result == AdamResponse.AdamDownNoChange);

        }

        [Test]
        [TestCase(8, 4, 5, false, 3)] // 8 line credit, fails on the 4th line
        [TestCase(4, 5, 0, true, 4)]  // 4 line credit, fails on header
        [TestCase(10, 10, 1, false, 9)] // 10 line credit, fails on the 10th line
        public void PodTransactionAdamExceptionNotOnFirstWriteSavesRemainingTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader, int errorTry)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();

            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            eventRepository.Setup(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var g = 1;
            command.Setup(m => m.ExecuteNonQuery()).Callback(() =>
            {
                if (g == errorTry)
                {
                    command.Setup(n => n.ExecuteNonQuery()).Throws(new Exception("seriously?"));
                }
                else
                {
                    g++;
                }
            });

            PodTransaction podTransaction = GetPodTransaction(numberOfLines);
            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.PodTransaction(podTransaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(podTransaction.CanWriteHeader == canWriteHeader);
            Assert.That(podTransaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()), Times.Once);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);

            Assert.That(result == AdamResponse.AdamDown);

        }

        [Test]
        [TestCase(2, 3)]
        [TestCase(10, 11)]
        public void SuccessfulPodWritesCorrectNumberOfLinesToAdam(int numberOfLines, int numberOfAdamExecutes)
        {
            var adamSettings = new AdamSettings();
            var podEvent = new PodEvent();
            var job = new Job();
            var podTransaction = GetPodTransaction(numberOfLines);
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            command.Setup(x => x.ExecuteNonQuery());

            podTransactionFactory.Setup(x => x.Build(It.IsAny<Job>(), It.IsAny<int>())).Returns(podTransaction);

            var result = moqAdamRepository.Object.Pod(podEvent, adamSettings, job);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(numberOfAdamExecutes));
            Assert.That(podTransaction.CanWriteHeader == true);
            Assert.That(podTransaction.LineSql.Count == 0);
            Assert.That(result == AdamResponse.Success);

        }

        [Test]
        [TestCase(8, 1, 8, false, AdamResponse.AdamDown)]
        [TestCase(0, 1, 0, true, AdamResponse.Unknown)] // 0 line credit, fails on the header
        public void PodAdamExceptionOnFirstWriteSavesCompletePodTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader, AdamResponse response)
        {
            var podEvent = new PodEvent();
            var job = new Job();
            var podTransaction = GetPodTransaction(numberOfLines);
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();

            podTransactionFactory.Setup(x => x.Build(It.IsAny<Job>(), It.IsAny<int>())).Returns(podTransaction);
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);
            eventRepository.Setup(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            command.Setup(m => m.ExecuteNonQuery()).Throws(new Exception("oops damn!"));

            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.Pod(podEvent, adamSettings, job);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(podTransaction.CanWriteHeader == canWriteHeader);
            Assert.That(podTransaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()), Times.Once);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            Assert.That(result == response);

        }

        [Test]
        [TestCase(8, 4, 5, false, 3, AdamResponse.AdamDown)]   // 8 line credit, fails on the 4th line
        [TestCase(4, 5, 0, true, 4, AdamResponse.Unknown)]     // 4 line credit, fails on header
        [TestCase(10, 10, 1, false, 9, AdamResponse.AdamDown)] // 10 line credit, fails on the 10th line
        public void PodAdamExceptionNotOnFirstWriteSavesRemainingTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader, int errorTry, AdamResponse response)
        {
            var podEvent = new PodEvent();
            var job = new Job();
            var podTransaction = GetPodTransaction(numberOfLines);
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            podTransactionFactory.Setup(x => x.Build(It.IsAny<Job>(), It.IsAny<int>())).Returns(podTransaction);

            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            eventRepository.Setup(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var g = 1;
            command.Setup(m => m.ExecuteNonQuery()).Callback(() =>
            {
                if (g == errorTry)
                {
                    command.Setup(n => n.ExecuteNonQuery()).Throws(new Exception("seriously?"));
                }
                else
                {
                    g++;
                }
            });

            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.Pod(podEvent, adamSettings, job);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(podTransaction.CanWriteHeader == canWriteHeader);
            Assert.That(podTransaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertPodTransaction(It.IsAny<PodTransaction>()), Times.Once);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);

            Assert.That(result == response);

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
        public void GrnDoesNotWriteLineToAdam()
        {
            var adamSettings = new AdamSettings();
            var grnEvent = new GrnEvent();
            var delivery = new DeliveryDetail { GrnNumber = "123456", AccountCode = "56666" };

            deliveryReadRepository.Setup(x => x.GetDeliveryById(It.IsAny<int>(), It.IsAny<string>())).Returns(delivery);

            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();
            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            command.Setup(m => m.ExecuteNonQuery()).Throws(new Exception("oops I did it again!"));

            var result = moqAdamRepository.Object.Grn(grnEvent, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Once);
            Assert.That(result == AdamResponse.AdamDown);

        }

        [Test]
        public void GrnNoNumberDoesNotWriteLineToAdam()
        {
            var adamSettings = new AdamSettings();
            var grnEvent = new GrnEvent();
            var delivery = new DeliveryDetail { GrnNumber = string.Empty, AccountCode = "56666" };

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
        [TestCase(8, 1, 8, false)]
        [TestCase(0, 1, 0, true)] // 0 line credit, fails on the header
        public void AmendmentExceptionOnFirstWriteSavesCompleteTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader)
        {
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();

            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);
            eventRepository.Setup(x => x.InsertAmendmentTransaction(It.IsAny<AmendmentTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            command.Setup(m => m.ExecuteNonQuery()).Throws(new Exception("oops damn!"));

            AmendmentTransaction transaction = GetAmendmentTransaction(numberOfLines);
            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.AmendmentTransaction(transaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(transaction.CanWriteHeader == canWriteHeader);
            Assert.That(transaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertAmendmentTransaction((It.IsAny<AmendmentTransaction>())), Times.Never);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            Assert.That(result == AdamResponse.AdamDownNoChange);

        }

        [Test]
        [TestCase(8, 4, 5, false, 3, AdamResponse.AdamDown)]   // 8 line amendment, fails on the 4th line
        [TestCase(4, 5, 0, true, 4, AdamResponse.AdamDown)]     // 4 line amendment, fails on header
        [TestCase(10, 10, 1, false, 9, AdamResponse.AdamDown)] // 10 line amendment, fails on the 10th line
        public void AmendmentExceptionNotOnFirstWriteSavesRemainingTransaction(int numberOfLines, int adamExecutesResult, int numberOfLinesToSave, bool canWriteHeader, int errorTry, AdamResponse response)
        {
            var transaction = GetAmendmentTransaction(numberOfLines);
            var connection = new Mock<DbConnection>();
            var command = new Mock<DbCommand>();

            moqAdamRepository.Setup(x => x.GetAdamConnection(It.IsAny<AdamSettings>()))
                .Returns(connection.Object);

            moqAdamRepository.Setup(x => x.GetAdamCommand(It.IsAny<DbConnection>()))
                         .Returns(command.Object);

            eventRepository.Setup(x => x.InsertAmendmentTransaction(It.IsAny<AmendmentTransaction>()));
            logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()));

            var g = 1;
            command.Setup(m => m.ExecuteNonQuery()).Callback(() =>
            {
                if (g == errorTry)
                {
                    command.Setup(n => n.ExecuteNonQuery()).Throws(new Exception("seriously?"));
                }
                else
                {
                    g++;
                }
            });

            var adamSettings = new AdamSettings();

            var result = moqAdamRepository.Object.AmendmentTransaction(transaction, adamSettings);

            command.Verify(x => x.ExecuteNonQuery(), Times.Exactly(adamExecutesResult));
            Assert.That(transaction.CanWriteHeader == canWriteHeader);
            Assert.That(transaction.LineSql.Count == numberOfLinesToSave);
            eventRepository.Verify(x => x.InsertAmendmentTransaction(It.IsAny<AmendmentTransaction>()), Times.Once);
            logger.Verify(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);

            Assert.That(result == response);

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