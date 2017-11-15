namespace PH.Well.UnitTests.Api.Infrastructure
{
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Internal;
    using Well.Api.Infrastructure;
    using Well.Api.Models;
    using Well.Domain.Enums;


    [TestFixture]
    public class WellApiConnectionStringFactoryTests
    {
        public class TheGetBranchConnectionsMethod : WellApiConnectionStringFactoryTests
        {
            [Test]
            public void TestBranchConnectionStrings()
            {
                var branchConnections = new WellApiConnectionStringFactory().GetBranchConnections();

                Assert.That(branchConnections.Count(x=> x.Type == ConnectionType.Dapper), Is.EqualTo(13));
                Assert.That(branchConnections.Count(x => x.Type == ConnectionType.Ef), Is.EqualTo(13));

                foreach (var branchConnection in branchConnections)
                {

                    switch ((Branch)branchConnection.BranchId)
                    {
                        case Branch.Medway:
                            AssertConnection(branchConnection, "Test1");
                            break;
                        case Branch.Coventry:
                            AssertConnection(branchConnection, "Test1");
                            break;
                        case Branch.Fareham:
                            AssertConnection(branchConnection, "Test1");
                            break;
                        case Branch.Dunfermline:
                            AssertConnection(branchConnection, "Test1");
                            break;
                        case Branch.Leeds:
                            AssertConnection(branchConnection, "Test1");
                            break;
                        case Branch.Hemel:
                            AssertConnection(branchConnection, "Test1");
                            break;
                        case Branch.Birtley:
                            AssertConnection(branchConnection, "Test2");
                            break;
                        case Branch.Belfast:
                            AssertConnection(branchConnection, "Test2");
                            break;
                        case Branch.Brandon:
                            AssertConnection(branchConnection, "Test2");
                            break;
                        case Branch.Plymouth:
                            AssertConnection(branchConnection, "Test2");
                            break;
                        case Branch.Bristol:
                            AssertConnection(branchConnection, "Test2");
                            break;
                        case Branch.Haydock:
                            AssertConnection(branchConnection, "Test2");
                            break;
                    }
                }

            }

            private void AssertConnection(BranchConnection branchConnection, string test1)
            {
                Assert.That(
                    branchConnection.ConnectionString, branchConnection.Type == ConnectionType.Dapper ?
                        Is.EqualTo($"{test1}Connection")
                        : Is.EqualTo($"{test1}EntitiesConnection")
                );
            }
        }
    }
}