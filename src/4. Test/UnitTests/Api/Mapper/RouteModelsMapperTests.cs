namespace PH.Well.UnitTests.Api.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Factories;
    using NUnit.Framework;
    using Well.Api.Mapper;
    using Well.Common.Extensions;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class RouteModelsMapperTests
    {
        public class TheMapMethod : RouteModelsMapperTests
        {
            [Test]
            public void ShouldMapRouteHeaderToRouteModel()
            {
                var routeHeader1 = RouteHeaderFactory.New.
                    With(x => x.RouteNumber = "2").
                    With(x => x.DriverName = "Chris").
                    With(x => x.RouteStatusCode = RouteStatusCode.Compl.ToString()).
                    With(x => x.DateUpdated = DateTime.Now).Build();

                routeHeader1.Stops.Add(StopFactory.New.With(x => x.StopPerformanceStatusCodeId = (int)PerformanceStatus.Compl).Build());
                routeHeader1.Stops.Add(StopFactory.New.With(x => x.StopPerformanceStatusCodeId = (int)PerformanceStatus.Abypa).Build());
                routeHeader1.Stops.Add(StopFactory.New.With(x => x.StopPerformanceStatusCodeId = (int)PerformanceStatus.Incom).Build());
                routeHeader1.Stops.Add(StopFactory.New.With(x => x.StopPerformanceStatusCodeId = (int)PerformanceStatus.Narri).Build());
                routeHeader1.Stops.Add(StopFactory.New.With(x => x.StopPerformanceStatusCodeId = (int)PerformanceStatus.Nbypa).Build());
                routeHeader1.Stops.Add(StopFactory.New.With(x => x.StopPerformanceStatusCodeId = (int)PerformanceStatus.Ndone).Build());
                routeHeader1.Stops.Add(StopFactory.New.With(x => x.StopPerformanceStatusCodeId = (int)PerformanceStatus.Notdef).Build());

                var routeHeaders = new List<RouteHeader>
                {
                    routeHeader1
                };

                var a = routeHeaders.FirstOrDefault().RouteStatus;

                var routeModels = new RouteModelsMapper().Map(routeHeaders);

                Assert.That(routeModels.Count, Is.EqualTo(1));
                var model = routeModels.First();
                Assert.That(model.Route, Is.EqualTo("2"));
                Assert.That(model.DriverName, Is.EqualTo("Chris"));
                Assert.That(model.RouteStatus, Is.EqualTo("In Progress"));
                Assert.That(model.DateTimeUpdated, Is.EqualTo(routeHeader1.DateUpdated));
                Assert.That(model.TotalDrops, Is.EqualTo(7));
                Assert.That(model.DeliveryExceptionCount, Is.EqualTo(6));
                Assert.That(model.DeliveryCleanCount, Is.EqualTo(1));
            }
        }
    }
}
