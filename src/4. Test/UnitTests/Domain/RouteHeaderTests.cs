using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.UnitTests.Domain
{
    using System.Collections.ObjectModel;
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

            [Test]
            public void CleanJobsCountIncludesAllCompleteJobs()
            {
                var routeHeader = new RouteHeader { Stops = new Collection<Stop>(new List<Stop>()
                {
                    new Stop()
                    {
                        Jobs = new Collection<Job>(new List<Job>()
                        {
                            new Job() {PerformanceStatus = PerformanceStatus.Compl},
                            new Job() {PerformanceStatus = PerformanceStatus.Compl},
                            new Job() {PerformanceStatus = PerformanceStatus.Resolved},
                        })
                    },
                    new Stop()
                    {
                        Jobs = new Collection<Job>(new List<Job>()
                        {
                            new Job() {PerformanceStatus = PerformanceStatus.Compl},
                            new Job() {PerformanceStatus = PerformanceStatus.Incom},
                            new Job() {PerformanceStatus = PerformanceStatus.Resolved},
                        })
                    }
                })};

                Assert.AreEqual(3,routeHeader.CleanJobs);
            }

            [Test]
            public void ExceptionJobsCountIncludesAllCompleteJobs()
            {
                var routeHeader = new RouteHeader
                {
                    Stops = new Collection<Stop>(new List<Stop>()
                    {
                        new Stop()
                        {
                            Jobs = new Collection<Job>(new List<Job>()
                            {
                                new Job() {PerformanceStatus = PerformanceStatus.Incom}, //Exception
                                new Job() {PerformanceStatus = PerformanceStatus.Abypa}, //Exception
                                new Job() {PerformanceStatus = PerformanceStatus.Resolved},
                            })
                        },
                        new Stop()
                        {
                            Jobs = new Collection<Job>(new List<Job>()
                            {
                                new Job() {PerformanceStatus = PerformanceStatus.Incom}, //Exception
                                new Job() {PerformanceStatus = PerformanceStatus.Nbypa}, //Exception
                                new Job() {PerformanceStatus = PerformanceStatus.Compl},
                            })
                        }
                    })
                };

                Assert.AreEqual(4, routeHeader.ExceptionJobs);
            }

        }
    }
}
