namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;

    using PH.Well.Repositories;

    using Repositories.Contracts;
    using Repositories.Read;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class DeliveryReadRepositoryTests
    {
        private Mock<ILogger> logger = new Mock<ILogger>(MockBehavior.Strict);
        private Mock<IDapperReadProxy> dapperProxy= new Mock<IDapperReadProxy>(MockBehavior.Strict);

        private DeliveryReadRepository repository;
        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperReadProxy>(MockBehavior.Strict);

            this.repository = new DeliveryReadRepository(this.logger.Object, this.dapperProxy.Object);
        }
  
    }
}