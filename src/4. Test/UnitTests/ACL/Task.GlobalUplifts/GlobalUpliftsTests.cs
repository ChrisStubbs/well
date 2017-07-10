using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories;
using PH.Well.Task.GlobalUplifts;
using PH.Well.Task.GlobalUplifts.Csv;
using PH.Well.Task.GlobalUplifts.Data;
using PH.Well.Task.GlobalUplifts.Import;

namespace PH.Well.UnitTests.ACL.Task.GlobalUplifts
{
    [TestFixture]
    public class GlobalUpliftsTests
    {
        private string _csvHeader = @"BRANCH,ACC NO,CREDIT REASON CODE,PRODUCT CODE,QTY,Start Date,End Date";

        [Test]
        public void AllFieldsInvalidTest()
        {
            var maxUpliftStartDate = DateTime.Parse("2017-03-08 16:00:00Z");
            var startDate = "2017-03-09 16:00:00Z";
            //15 days difference 
            var endDate = "2017-03-24 16:00:00Z";

            var sb = new StringBuilder(_csvHeader);
            sb.AppendLine();
            sb.AppendLine($"a,,a,a,0,{startDate},{endDate}");
            var csvString = sb.ToString();

            var provider = new CsvUpliftDataProvider(new StringReader(csvString))
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
            var startDate = "2017-03-08 16:00:00Z";
            //14 days difference 
            var endDate = "2017-03-22 16:00:00Z";

            var sb = new StringBuilder(_csvHeader);
            sb.AppendLine();
            sb.AppendLine($"1,123.000,global uplift,123,1,{startDate},{endDate}");
            var csvString = sb.ToString();

            var provider = new CsvUpliftDataProvider(new StringReader(csvString))
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
            var directoryProvider = new DirectoryCsvUpliftDataProvider(path);
            var dataSets = directoryProvider.GetUpliftData().ToList();
            Assert.That(dataSets.Count == 2);
        }

        [Test]
        public void GlobalUpliftsTaskTest()
        {
            var importService = new Mock<IUpliftDataImportService>();
            var task = new UpliftImportTask(importService.Object);

            var directoryPath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), @"ACL\Task.GlobalUplifts");
            task.Execute(new UpliftImportTaskData {Directories = new[] {directoryPath}.ToList()});

            //Verify that import has been called once. (1 directory csv provider per directory)
            importService.Verify(x => x.Import(It.IsAny<IUpliftDataProvider>()), Times.Once);
        }

        [Test]
        public void GlobalUpliftTransactionFactoryTest()
        {
            var transaction = new GlobalUpliftTransaction(101231, 123, "123.001", "Global Uplift", 14472, 2,
                DateTime.Now, DateTime.Now.AddHours(1));
            var globalUpliftTransactionFactory = new GlobalUpliftTransactionFactory();

            var lineSql = globalUpliftTransactionFactory.LineSql(transaction);
            var headerSql = globalUpliftTransactionFactory.HeaderSql(transaction);

        }

        [Test]
        public void GlobalUpliftTransactionFactoryShouldThrowException()
        {
            //Transaction specifies that header and line shouldn't be written
            var transaction = new GlobalUpliftTransaction(101231, 123, "123.001", "Global Uplift", 14472, 2,
                DateTime.Now, DateTime.Now.AddHours(1), false, false);

            var globalUpliftTransactionFactory = new GlobalUpliftTransactionFactory();

            Assert.Throws<InvalidOperationException>(() => globalUpliftTransactionFactory.LineSql(transaction));
            Assert.Throws<InvalidOperationException>(() => globalUpliftTransactionFactory.HeaderSql(transaction));
        }
    }
}
