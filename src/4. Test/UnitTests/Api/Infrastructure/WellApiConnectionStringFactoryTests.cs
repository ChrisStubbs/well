namespace PH.Well.UnitTests.Api.Infrastructure
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Internal;
    using Well.Api.Infrastructure;
    using Well.Api.Models;

    [TestFixture]
    public class WellApiConnectionStringFactoryTests
    {
        public class TheGetBranchConnectionsMethod : WellApiConnectionStringFactoryTests
        {
            [Test]
            public void Should_Load_Connection_Fromm_Config()
            {
                var branchConnections = new WellApiConnectionStringFactory().BranchConnections;

                Assert.That(branchConnections.Count(), Is.EqualTo(2));
            }

            [Test]
            public void Should_Get_Dapper_Connection()
            {
                IConnectionStringFactory sut = new WellApiConnectionStringFactory();
                var cnn = sut.GetConnectionString(1, ConnectionType.Dapper);

                Assert.That(cnn, Is.EqualTo(ConfigurationManager.ConnectionStrings["Test1"].ConnectionString));

                cnn = sut.GetConnectionString(4, ConnectionType.Dapper);
                Assert.That(cnn, Is.EqualTo(ConfigurationManager.ConnectionStrings["Test2"].ConnectionString));
            }

            [Test]
            public void Should_Get_Ef_Connection()
            {
                IConnectionStringFactory sut = new WellApiConnectionStringFactory();
                var cnn = sut.GetConnectionString(1, ConnectionType.Ef);

                Assert.That(cnn, Is.EqualTo(ConfigurationManager.ConnectionStrings["Test1Entities"].ConnectionString));

                cnn = sut.GetConnectionString(4, ConnectionType.Ef);
                Assert.That(cnn, Is.EqualTo(ConfigurationManager.ConnectionStrings["Test2Entities"].ConnectionString));
            }

            [Test]
            public void Should_Get_Default_If_No_Branch()
            {
                IConnectionStringFactory sut = new WellApiConnectionStringFactory();
                var cnn = sut.GetConnectionString(null, ConnectionType.Dapper);

                Assert.That(cnn, Is.EqualTo(ConfigurationManager.ConnectionStrings["Well"].ConnectionString));

                cnn = sut.GetConnectionString(null, ConnectionType.Ef);
                Assert.That(cnn, Is.EqualTo(ConfigurationManager.ConnectionStrings["WellEntities"].ConnectionString));
            }

            [Test]
            public void Should_Get_All_DapperConnections()
            {
                IConnectionStringFactory sut = new WellApiConnectionStringFactory();

                var all = sut.GetConnectionStrings(ConnectionType.Dapper);
                var config = new List<string>
                {
                    ConfigurationManager.ConnectionStrings["Test1"].ConnectionString,
                    ConfigurationManager.ConnectionStrings["Test2"].ConnectionString
                };

                Assert.That(all, Is.EquivalentTo(config));
            }

            [Test]
            public void Should_Get_All_EfConnections()
            {
                IConnectionStringFactory sut = new WellApiConnectionStringFactory();

                var all = sut.GetConnectionStrings(ConnectionType.Ef);
                var config = new List<string>
                {
                    ConfigurationManager.ConnectionStrings["Test1Entities"].ConnectionString,
                    ConfigurationManager.ConnectionStrings["Test2Entities"].ConnectionString
                };

                Assert.That(all, Is.EquivalentTo(config));
            }
        }
    }
}