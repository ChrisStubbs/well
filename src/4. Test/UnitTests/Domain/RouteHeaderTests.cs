namespace PH.Well.UnitTests.Domain
{
    using NUnit.Framework;
    using Well.Domain;

    [TestFixture]
    public class RouteHeaderTests
    {

        public class TryParseBranchIdFromRouteNumber : RouteHeaderTests
        {

            [Test]
            public void BranchIdShouldBeFirstTwoCharactersOfRoutNumber()
            {
                var routeHeader = new RouteHeader {RouteNumber = "1234"};
                var branchId = 0;

                Assert.True(routeHeader.TryParseBranchIdFromRouteNumber(out branchId));

                Assert.That(branchId,Is.EqualTo(12));

            }

        }
    }
}
