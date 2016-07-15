using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.UnitTests.Domain
{
    using NUnit.Framework;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class RouteHeaderTests
    {
        public class TheRouteStatusCodeMethods : RouteHeaderTests
        {
            [Test]
            public void ShouldSetRouteStatusToNoDefIfNull()
            {
                var routeHeader = new RouteHeader {RouteStatusCode = null};
                Assert.That(routeHeader.RouteStatus,Is.EqualTo(RouteStatusCode.Notdef));
            }

            [Test]
            public void ShouldSetRouteStatusToNoDefIfRubishData()
            {
                var routeHeader = new RouteHeader { RouteStatusCode = "this is not a enum name" };
                Assert.That(routeHeader.RouteStatus, Is.EqualTo(RouteStatusCode.Notdef));
            }

        }
    }
}
