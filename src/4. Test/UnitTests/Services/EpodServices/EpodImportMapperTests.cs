using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.UnitTests.Services.EpodServices
{
    using NUnit.Framework;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Services.EpodServices;

    [TestFixture]
    public class EpodImportMapperTests 
    {
        [Test]
        public void MergeRouteHeaderTests()
        {

            var from = new RouteHeader();
            from.RouteStatusCode = "Compl";
            from.RouteStatusDescription = "Completed";
            from.PerformanceStatusCode = "InProg";
            from.PerformanceStatusDescription = "In Progress";
            from.AuthByPass = 11;
            from.NonAuthByPass = 45;
            from.ShortDeliveries = 3;
            from.DamagesRejected = 3;
            from.DamagesAccepted = 1;
            from.ActualStopsCompleted = 800;

            var to = new RouteHeader
            {
                Id = 24,
                RouteOwnerId = 25,
                RouteNumber = "30"
            };

            new EpodImportMapper().MergeRouteHeader(from, to);

            Assert.That(to.RouteStatusCode, Is.EqualTo(from.RouteStatusCode));
            Assert.That(to.RouteStatusDescription, Is.EqualTo(from.RouteStatusDescription));
            Assert.That(to.PerformanceStatusCode, Is.EqualTo(from.PerformanceStatusCode));
            Assert.That(to.PerformanceStatusDescription, Is.EqualTo(from.PerformanceStatusDescription));
            Assert.That(to.AuthByPass, Is.EqualTo(from.AuthByPass));
            Assert.That(to.NonAuthByPass, Is.EqualTo(from.NonAuthByPass));
            Assert.That(to.ShortDeliveries, Is.EqualTo(from.ShortDeliveries));
            Assert.That(to.DamagesRejected, Is.EqualTo(from.DamagesRejected));
            Assert.That(to.DamagesAccepted, Is.EqualTo(from.DamagesAccepted));
            Assert.That(to.ActualStopsCompleted, Is.EqualTo(from.ActualStopsCompleted));

            Assert.That(from.Id,Is.EqualTo(to.Id));
            Assert.That(from.RouteOwnerId, Is.EqualTo(to.RouteOwnerId));
            Assert.That(from.RouteNumber, Is.EqualTo(to.RouteNumber));
        }

        [Test]
        public void TheMapStopMethod()
        {
            var from = new Stop();

            from.StopStatusCode = "Something";
            from.StopStatusDescription = "Something else";
            from.PerformanceStatusCode = "not sure";
            from.PerformanceStatusDescription = "no i don't know";
            from.StopByPassReason = "Something";

            var to = new Stop();

            new EpodImportMapper().MapStop(from, to);

            Assert.That(to.StopStatusCode, Is.EqualTo(from.StopStatusCode));
            Assert.That(to.StopStatusDescription, Is.EqualTo(from.StopStatusDescription));
            Assert.That(to.PerformanceStatusCode, Is.EqualTo(from.PerformanceStatusCode));
            Assert.That(to.PerformanceStatusDescription, Is.EqualTo(from.PerformanceStatusDescription));
            Assert.That(to.StopByPassReason, Is.EqualTo(from.StopByPassReason));
        }


        [Test]
        public void JobMapper()
        {
            var from = new Job();
            var to = new Job();

            from.JobByPassReason = "Some reason";
            from.PerformanceStatus = PerformanceStatus.Abypa;
            from.InvoiceNumber = "12009";

            new EpodImportMapper().MapJob(from, to);

            Assert.That(to.JobByPassReason, Is.EqualTo(from.JobByPassReason));
            Assert.That(to.PerformanceStatus, Is.EqualTo(from.PerformanceStatus));
            Assert.That(to.InvoiceNumber, Is.EqualTo(from.InvoiceNumber));
        }
    }
}
