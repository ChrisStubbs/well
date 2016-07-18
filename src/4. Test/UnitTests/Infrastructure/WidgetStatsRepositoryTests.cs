namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using Common.Contracts;
    using Domain;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class WidgetStatsRepositoryTests
    {
        private Mock<ILogger> logger;
        private Mock<IWellDapperProxy> dapperProxy;
        private WidgetStatsRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new WidgetStatsRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetWidgetStatsMethod : WidgetStatsRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var widgetStats = WidgetStatsFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure("WidgetStats_Get")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<WidgetStats>()).Returns(new List<WidgetStats> { widgetStats });

                var result = repository.GetWidgetStats();

                dapperProxy.Verify(x => x.WithStoredProcedure("WidgetStats_Get"), Times.Once);
                dapperProxy.Verify(x => x.Query<WidgetStats>(), Times.Once);

                Assert.That(result, Is.EqualTo(widgetStats));
            }
        }

    }
}
