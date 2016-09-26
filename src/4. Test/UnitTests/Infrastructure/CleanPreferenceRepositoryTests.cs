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
    public class CleanPreferenceRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private CleanPreferenceRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);

            this.repository = new CleanPreferenceRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetAllMethod : CleanPreferenceRepositoryTests
        {
            [Test]
            public void ShouldReturnAllCleanPreferences()
            {
                var cleanPreferences = new List<CleanPreference>();

                var branch = BranchFactory.New.Build();

                cleanPreferences.Add(CleanPreferenceFactory.New.WithBranch(branch).Build());

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesGetAll))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                    x => x.AddParameter("cleanPreferenceId", cleanPreferences[0].Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<CleanPreference>()).Returns(cleanPreferences);

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesBranchesGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<Branch>()).Returns(new List<Branch>());

                var cleans = this.repository.GetAll();

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesGetAll), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("cleanPreferenceId", cleanPreferences[0].Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.CleanPreferencesBranchesGet), Times.Once);

                this.dapperProxy.Verify(x => x.Query<Branch>(), Times.Once);

                Assert.That(cleans.Count(), Is.EqualTo(1));
            }
        }
    }
}