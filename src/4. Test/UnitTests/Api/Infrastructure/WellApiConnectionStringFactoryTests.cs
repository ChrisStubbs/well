namespace PH.Well.UnitTests.Api.Infrastructure
{
    using NUnit.Framework;
    using NUnit.Framework.Internal;
    using Well.Api.Infrastructure;
    using Well.Domain.Enums;


    [TestFixture]
    public class WellApiConnectionStringFactoryTests
    {
        public class TheGetBranchConnectionsMethod : WellApiConnectionStringFactoryTests
        {
            [Test]
            public void Test()
            {
                var branchConnections = new WellApiConnectionStringFactory().GetBranchConnections();

                foreach (var branchConnection in branchConnections)
                {

                    switch ((Branch)branchConnection.BranchId)
                    {
                        case Branch.Medway:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test1Connection"));
                            break;
                        case Branch.Coventry:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test1Connection"));
                            break;
                        case Branch.Fareham:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test1Connection"));
                            break;
                        case Branch.Dunfermline:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test1Connection"));
                            break;
                        case Branch.Leeds:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test1Connection"));
                            break;
                        case Branch.Hemel:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test1Connection"));
                            break;
                        case Branch.Birtley:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test2Connection"));
                            break;
                        case Branch.Belfast:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test2Connection"));
                            break;
                        case Branch.Brandon:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test2Connection"));
                            break;
                        case Branch.Plymouth:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test2Connection"));
                            break;
                        case Branch.Bristol:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test2Connection"));
                            break;
                        case Branch.Haydock:
                            Assert.That(branchConnection.ConnectionString, Is.EqualTo("Test2Connection"));
                            break;
                    }
                }

            }
        }
    }
}