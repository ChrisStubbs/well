namespace PH.Well.UnitTests.Domain
{
    using System.Collections.Generic;

    using NUnit.Framework;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class RouteHeaderTests
    {
        public class TheRouteStatusCodeMethods : RouteHeaderTests
        {
            [Test]
            public void CleanJobsCountIncludesAllCompleteJobs()
            {
                var routeHeader = new RouteHeader { Stops = new List<Stop>(new List<Stop>()
                {
                    new Stop()
                    {
                        Jobs = new List<Job>(new List<Job>()
                        {
                            new Job() {PerformanceStatus = PerformanceStatus.Compl},
                            new Job() {PerformanceStatus = PerformanceStatus.Compl},
                            new Job() {PerformanceStatus = PerformanceStatus.Resolved},
                        })
                    },
                    new Stop()
                    {
                        Jobs = new List<Job>(new List<Job>()
                        {
                            new Job() {PerformanceStatus = PerformanceStatus.Compl},
                            new Job() {PerformanceStatus = PerformanceStatus.Incom},
                            new Job() {PerformanceStatus = PerformanceStatus.Resolved},
                        })
                    }
                })};

                Assert.AreEqual(3,routeHeader.CleanJobs);
            }

        }
    }
}
