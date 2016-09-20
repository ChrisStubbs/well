namespace PH.Well.UnitTests.Infrastructure
{
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
    }
}