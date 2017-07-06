﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PH.Well.Task.GlobalUplifts.Csv;

namespace PH.Well.UnitTests.ACL.Task.GlobalUplifts
{
    [TestFixture]
    public class CsvDataProviderTests
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
            var dataSet = provider.GetUpliftData();

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
            var dataSet = provider.GetUpliftData();

            Assert.AreEqual(1, dataSet.Records.Count());
            Assert.AreEqual(0, dataSet.Errors.Count());
            Assert.False(dataSet.HasErrors);
        }
    }
}
