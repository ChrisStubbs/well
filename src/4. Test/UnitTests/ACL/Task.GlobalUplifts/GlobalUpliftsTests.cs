using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PH.Well.Common.Contracts;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories;
using PH.Well.Repositories.Contracts;
using PH.Well.Task.GlobalUplifts;
using PH.Well.Task.GlobalUplifts.Csv;
using PH.Well.Task.GlobalUplifts.Data;
using PH.Well.Task.GlobalUplifts.Import;

namespace PH.Well.UnitTests.ACL.Task.GlobalUplifts
{
    [TestFixture]
    public class GlobalUpliftsTests
    {
        private string _csvHeader = @"BRANCH,ACC NO,CREDIT REASON CODE,PRODUCT CODE,QTY,Start Date,End Date,Ref";

        [Test]
        public void AllFieldsInvalidTest()
        {
            var maxUpliftStartDate = DateTime.Parse("2017-03-08 16:00:00Z");
            var startDate = "2017-03-07 16:00:00Z";
            //15 days difference 
            var endDate = "2017-03-24 16:00:00Z";

            var sb = new StringBuilder(_csvHeader);
            sb.AppendLine();
            sb.AppendLine($"a,,a,a,0,{startDate},{endDate}");
            var csvString = sb.ToString();

            var provider = new CsvUpliftDataProvider(DateTime.Now.ToString(),new StringReader(csvString))
            {
                MaxUpliftStartDate = maxUpliftStartDate
            };
            var dataSet = provider.GetUpliftData().Single();

            Assert.AreEqual(0, dataSet.Records.Count());
            Assert.AreEqual(1, dataSet.Errors.Count());
            Assert.True(dataSet.HasErrors);

        }

        [Test]
        public void AllFieldsValidTest()
        {
            var maxUpliftStartDate = DateTime.Parse("2017-03-09 16:00:00Z");
            var startDate = "2017-03-09 16:00:00Z";

            //14 days difference 
            var endDate = "2017-03-22 16:00:00Z";

            var sb = new StringBuilder(_csvHeader);
            sb.AppendLine();
            sb.AppendLine($"1,123.000,global uplift,123,1,{startDate},{endDate}");
            var csvString = sb.ToString();

            var provider = new CsvUpliftDataProvider(DateTime.Now.ToString(), new StringReader(csvString))
            {
                MaxUpliftStartDate = maxUpliftStartDate
            };
            var dataSet = provider.GetUpliftData().Single();

            Assert.AreEqual(1, dataSet.Records.Count());
            Assert.AreEqual(0, dataSet.Errors.Count());
            Assert.False(dataSet.HasErrors);
        }

        [Test]
        public void DirectoryCsvDataProviderTest()
        {
            var path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) , @"ACL\Task.GlobalUplifts");
            var directoryProvider = new DirectoryCsvUpliftDataProvider(path, path);
            var dataSets = directoryProvider.GetUpliftData().ToList();
            Assert.That(dataSets.Count == 2);
        }

        [Test]
        public void GlobalUpliftsTaskTest()
        {
            var importService = new Mock<IUpliftDataImportService>();
            var task = new UpliftImportTask(importService.Object);

            var directoryPath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), @"ACL\Task.GlobalUplifts");
            task.Execute(new UpliftImportTaskData
            {
                Directories = new[] {directoryPath}.ToList(),
                ArchiveDirectory = directoryPath
            });

            //Verify that import has been called once. (1 directory csv provider per directory)
            importService.Verify(x => x.Import(It.IsAny<IUpliftDataProvider>()), Times.Once);
        }

        [Test]
        public void GlobalUpliftTransactionFactoryTest()
        {
            var transaction = new GlobalUpliftTransaction(101231, 123, "123.001", "Global Uplift", 14472, 2,
                DateTime.Now, DateTime.Now.AddHours(1), 123456, "customer ref");
            var globalUpliftTransactionFactory = new GlobalUpliftTransactionFactory();

            var lineSql = globalUpliftTransactionFactory.LineSql(transaction);
            var headerSql = globalUpliftTransactionFactory.HeaderSql(transaction);

        }

        [Test]
        public void GlobalUpliftTransactionFactoryShouldThrowException()
        {
            //Transaction specifies that header and line shouldn't be written
            var transaction = new GlobalUpliftTransaction(101231, 123, "123.001", "Global Uplift", 14472, 2,
                DateTime.Now, DateTime.Now.AddHours(1), false, false, 0, "customer ref");

            var globalUpliftTransactionFactory = new GlobalUpliftTransactionFactory();

            Assert.Throws<InvalidOperationException>(() => globalUpliftTransactionFactory.LineSql(transaction));
            Assert.Throws<InvalidOperationException>(() => globalUpliftTransactionFactory.HeaderSql(transaction));
        }
    }


    [TestFixture]
    public class GlobalUpliftRepositoryTests
    {
        private Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private Mock<IJobRepository> _jobRepositoryMock = new Mock<IJobRepository>();
        private Mock<IEventLogger> _eventLoggerMock = new Mock<IEventLogger>();
        private Mock<IPodTransactionFactory> _podTransactionFactoryMock = new Mock<IPodTransactionFactory>();
        private Mock<IDeliveryReadRepository> _deliveryReadRepositoryMock = new Mock<IDeliveryReadRepository>();
        private Mock<IExceptionEventRepository> _eventRepositoryMock = new Mock<IExceptionEventRepository>();

        [Test]
        public void ShouldWriteLineAndHeaderTest()
        {
            var _globalUpliftTransactionFactoryMock = new Mock<IGlobalUpliftTransactionFactory>();

            var _adamRepositoryMock = new Mock<AdamRepository>(_loggerMock.Object, _jobRepositoryMock.Object,
                _eventLoggerMock.Object, _podTransactionFactoryMock.Object, _deliveryReadRepositoryMock.Object,
                _eventRepositoryMock.Object, _globalUpliftTransactionFactoryMock.Object)
            {
                CallBase = true
            };

            var transaction = new GlobalUpliftTransaction(1, 1, "123", "Global Uplift", 123, 1, DateTime.Now,
                DateTime.Now.AddDays(1), 0, "customer ref");

            _adamRepositoryMock
                .Setup(x => x.WriteGlobalUpliftLine(It.IsAny<GlobalUpliftTransaction>(), It.IsAny<AdamSettings>()))
                .Callback(() =>
                {
                    _globalUpliftTransactionFactoryMock.Object.LineSql(transaction);
                    transaction.LineDidWrite = true;
                })
                .Returns(AdamResponse.Success);

            _adamRepositoryMock
                .Setup(x => x.WriteGlobalUpliftHeader(It.IsAny<GlobalUpliftTransaction>(), It.IsAny<AdamSettings>()))
                .Callback(() =>
                {
                    _globalUpliftTransactionFactoryMock.Object.HeaderSql(transaction);
                    transaction.HeaderDidWrite = true;
                })
                .Returns(AdamResponse.Success);

            _adamRepositoryMock.Object.GlobalUplift(transaction, new AdamSettings());

            _globalUpliftTransactionFactoryMock.Verify(x => x.LineSql(transaction), Times.Once);
            _globalUpliftTransactionFactoryMock.Verify(x => x.HeaderSql(transaction), Times.Once);

            _adamRepositoryMock.Verify(x=> x.WriteGlobalUpliftLine(transaction,It.IsAny<AdamSettings>()),Times.Once);
            _adamRepositoryMock.Verify(x=> x.WriteGlobalUpliftHeader(transaction,It.IsAny<AdamSettings>()),Times.Once);

            Assert.True(transaction.LineDidWrite);
            Assert.True(transaction.HeaderDidWrite);
        }

        [Test]
        public void ShouldWriteHeaderOnlyTest()
        {
            var _globalUpliftTransactionFactoryMock = new Mock<IGlobalUpliftTransactionFactory>();

            var _adamRepositoryMock = new Mock<AdamRepository>(_loggerMock.Object, _jobRepositoryMock.Object,
                _eventLoggerMock.Object, _podTransactionFactoryMock.Object, _deliveryReadRepositoryMock.Object,
                _eventRepositoryMock.Object, _globalUpliftTransactionFactoryMock.Object)
            {
                CallBase = true
            };

            //Create transaction that specifies to write header only
            var transaction = new GlobalUpliftTransaction(1, 1, "123", "Global Uplift", 123, 1, DateTime.Now,
                DateTime.Now.AddDays(1), false, true, 0, "customer ref");

            _adamRepositoryMock
                .Setup(x => x.WriteGlobalUpliftLine(It.IsAny<GlobalUpliftTransaction>(), It.IsAny<AdamSettings>()))
                .Callback(() =>
                {
                    _globalUpliftTransactionFactoryMock.Object.LineSql(transaction);
                    transaction.LineDidWrite = true;
                })
                .Returns(AdamResponse.Success);

            _adamRepositoryMock
                .Setup(x => x.WriteGlobalUpliftHeader(It.IsAny<GlobalUpliftTransaction>(), It.IsAny<AdamSettings>()))
                .Callback(() =>
                {
                    _globalUpliftTransactionFactoryMock.Object.HeaderSql(transaction);
                    transaction.HeaderDidWrite = true;
                })
                .Returns(AdamResponse.Success);

            _adamRepositoryMock.Object.GlobalUplift(transaction, new AdamSettings());

            _globalUpliftTransactionFactoryMock.Verify(x => x.LineSql(transaction), Times.Never);
            _globalUpliftTransactionFactoryMock.Verify(x => x.HeaderSql(transaction), Times.Once);

            _adamRepositoryMock.Verify(x => x.WriteGlobalUpliftLine(transaction, It.IsAny<AdamSettings>()), Times.Never);
            _adamRepositoryMock.Verify(x => x.WriteGlobalUpliftHeader(transaction, It.IsAny<AdamSettings>()), Times.Once);

            Assert.False(transaction.WriteLine);
            Assert.False(transaction.LineDidWrite);

            Assert.True(transaction.WriteHeader);
            Assert.True(transaction.HeaderDidWrite);
        }

        [Test]
        public void ShouldCreateEventForFailedTransactionTest()
        {
            var _globalUpliftTransactionFactoryMock = new Mock<IGlobalUpliftTransactionFactory>();

            var _adamRepositoryMock = new Mock<AdamRepository>(_loggerMock.Object, _jobRepositoryMock.Object,
                _eventLoggerMock.Object, _podTransactionFactoryMock.Object, _deliveryReadRepositoryMock.Object,
                _eventRepositoryMock.Object, _globalUpliftTransactionFactoryMock.Object)
            {
                CallBase = true
            };

            var transaction = new GlobalUpliftTransaction(1, 1, "123", "Global Uplift", 123, 1, DateTime.Now,
                DateTime.Now.AddDays(1), 0, "customer ref");

            _adamRepositoryMock
                .Setup(x => x.WriteGlobalUpliftLine(It.IsAny<GlobalUpliftTransaction>(), It.IsAny<AdamSettings>()))
                .Callback(() =>
                {
                    _globalUpliftTransactionFactoryMock.Object.LineSql(transaction);
                })
                .Returns(AdamResponse.AdamDown);

            _adamRepositoryMock
                .Setup(x => x.WriteGlobalUpliftHeader(It.IsAny<GlobalUpliftTransaction>(), It.IsAny<AdamSettings>()))
                .Callback(() =>
                {
                    _globalUpliftTransactionFactoryMock.Object.HeaderSql(transaction);
                })
                .Returns(AdamResponse.Unknown);

            _adamRepositoryMock.Object.GlobalUplift(transaction, new AdamSettings());

            _globalUpliftTransactionFactoryMock.Verify(x => x.LineSql(transaction), Times.Once);
            _globalUpliftTransactionFactoryMock.Verify(x => x.HeaderSql(transaction), Times.Never);

            _adamRepositoryMock.Verify(x => x.WriteGlobalUpliftLine(transaction, It.IsAny<AdamSettings>()), Times.Once);
            _adamRepositoryMock.Verify(x => x.WriteGlobalUpliftHeader(transaction, It.IsAny<AdamSettings>()), Times.Never);

            Assert.False(transaction.LineDidWrite);
            Assert.False(transaction.HeaderDidWrite);

            _eventRepositoryMock.Verify(x => x.InsertGlobalUpliftEvent(It.IsAny<GlobalUpliftEvent>()), Times.Once);
        }
    }
}
