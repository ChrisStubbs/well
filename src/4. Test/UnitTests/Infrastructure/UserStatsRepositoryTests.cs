namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class UserStatsRepositoryTests
    {
        private Mock<IDapperProxy> dapperProxy;

        private UserStatsRepository repository;

        [SetUp]
        public void Setup()
        {
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);

            this.repository = new UserStatsRepository(this.dapperProxy.Object);
        }
    }
}