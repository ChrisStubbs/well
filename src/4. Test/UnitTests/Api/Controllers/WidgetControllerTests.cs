using PH.Well.Repositories.Contracts;

namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Security.Principal;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using Moq;
    using NUnit.Framework;
    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;
    using Well.Api.Models;

    [TestFixture]
    public class WidgetControllerTests : BaseControllerTests<WidgetController>
    {
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;
        private Mock<IUserStatsRepository> userStatsRepository;

        [SetUp]
        public void Setup()
        {
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            userStatsRepository = new Mock<IUserStatsRepository>(MockBehavior.Strict);

            this.Controller = new WidgetController(serverErrorResponseHandler.Object, userStatsRepository.Object);
            SetupController();
        }

        public class TheGetMethod : WidgetControllerTests
        {
            [Test]
            public void HasGetAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Get());

                var routeAttribute = GetAttributes<HttpGetAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
            }

            [Test]
            public void HasCorrectRouteAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Get());

                var routeAttribute = GetAttributes<RouteAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
                Assert.AreEqual("widgets", routeAttribute.Template);
            }

            [Test]
            public void ReturnsWidgetsWithCorrectUserStats()
            {
                string userIdentity = "bob";
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userIdentity), new[] {"A role"});

                var stats = new UserStats()
                {
                    AssignedCount = 5,
                    ExceptionCount = 6,
                    NotificationsCount = 7,
                    OutstandingCount = 8
                };
                userStatsRepository.Setup(r => r.GetByUser(userIdentity)).Returns(stats);

                var result = Controller.Get();

                var response = GetResponseObject<IEnumerable<WidgetModel>>(result);

                Assert.AreEqual(stats.ExceptionCount, response.SingleOrDefault(r => r.Name == "Exceptions").Count);
                Assert.AreEqual(stats.AssignedCount, response.SingleOrDefault(r => r.Name == "Assigned").Count);
                Assert.AreEqual(stats.NotificationsCount, response.SingleOrDefault(r => r.Name == "Notifications").Count);
                Assert.AreEqual(stats.OutstandingCount, response.SingleOrDefault(r => r.Name == "Outstanding").Count);

            }
        }
    }
}