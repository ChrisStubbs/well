﻿namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class SeasonalDateRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private SeasonalDateRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new SeasonalDateRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetAllMethod : SeasonalDateRepositoryTests
        {
            [Test]
            public void ShouldReturnAllSeasonalDates()
            {
                var seasonalDates = new List<SeasonalDate> { SeasonalDateFactory.New.Build() };

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesGetAll))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<SeasonalDate>()).Returns(seasonalDates);

                var seasonal = this.repository.GetAll();

                Assert.That(seasonal.Count(), Is.EqualTo(1));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesGetAll), Times.Once);

                this.dapperProxy.Verify(x => x.Query<SeasonalDate>(), Times.Once);
            }
        }

        public class TheDeleteMethod : SeasonalDateRepositoryTests
        {
            [Test]
            public void ShouldHardDeleteTheSeasonalDate()
            {
                var id = 12;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesDelete))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Delete(id);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesDelete), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

        public class TheSaveMethod : SeasonalDateRepositoryTests
        {
            [Test]
            public void ShouldSaveTheSeasonalDateAndItsBranchAssociations()
            {
                var branch1 = new BranchFactory().Build();
                var branch2 = new BranchFactory().Build();
                var seasonalDate = SeasonalDateFactory.New.WithBranch(branch1).WithBranch(branch2).Build();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("Description", seasonalDate.Description, DbType.String, 255))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("From", seasonalDate.From, DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("To", seasonalDate.To, DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("CreatedBy", this.repository.CurrentUser, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("UpdatedBy", this.repository.CurrentUser, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new[] { 1 });

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesToBranchSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("BranchId", branch1.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("SeasonalDateId", 1, DbType.Int32, null)).Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Save(seasonalDate);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesSave), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Description", seasonalDate.Description, DbType.String, 255), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("From", seasonalDate.From, DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("To", seasonalDate.To, DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.SeasonalDatesToBranchSave), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.AddParameter("BranchId", branch1.Id, DbType.Int32, null), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.AddParameter("SeasonalDateId", 1, DbType.Int32, null), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.Execute(), Times.Exactly(2));
            }
        }
    }
}