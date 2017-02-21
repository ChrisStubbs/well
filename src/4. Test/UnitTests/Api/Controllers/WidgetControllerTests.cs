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
    using Well.Api.Mapper.Contracts;
    using Well.Api.Models;
    using Well.Api.Validators.Contracts;

    [TestFixture]
    public class WidgetControllerTests : BaseControllerTests<WidgetController>
    {
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;
        private Mock<ILogger> logger;
        private Mock<IUserStatsRepository> userStatsRepository;
        private Mock<IWidgetRepository> widgetRepository;
        private Mock<IWidgetWarningMapper> mapper;
        private Mock<IWidgetWarningValidator> validator;
        private Mock<INotificationRepository> notificationsRepository;
        private Mock<IDeliveryReadRepository> deliveryReadRepository;

        [SetUp]
        public void Setup()
        {
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            logger = new Mock<ILogger>(MockBehavior.Strict);
            userStatsRepository = new Mock<IUserStatsRepository>(MockBehavior.Strict);
            widgetRepository = new Mock<IWidgetRepository>(MockBehavior.Loose);

            mapper = new Mock<IWidgetWarningMapper>(MockBehavior.Strict);
            validator = new Mock<IWidgetWarningValidator>(MockBehavior.Strict);

            notificationsRepository = new Mock<INotificationRepository>(MockBehavior.Strict);
            deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);

            this.Controller = new WidgetController(serverErrorResponseHandler.Object,
                logger.Object,
                userStatsRepository.Object,
                widgetRepository.Object,
                mapper.Object,
                validator.Object,
                notificationsRepository.Object,
                deliveryReadRepository.Object);
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
                //TODO - Update this test

                string userIdentity = "bob";
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userIdentity), new[] { "A role" });

                var warnings = new WidgetWarningLevels()
                {
                    ExceptionWarningLevel = 2,
                    OutstandingWarningLevel = 2,
                    AssignedWarningLevel = 2,
                    NotificationsWarningLevel = 2
                };

                deliveryReadRepository.Setup(d => d.GetExceptionDeliveries(userIdentity, true))
                    .Returns(new List<Delivery>());
                notificationsRepository.Setup(n => n.GetNotifications()).Returns(new List<Notification>());

                this.userStatsRepository.Setup(r => r.GetWidgetWarningLevels(userIdentity)).Returns(warnings);

                var result = Controller.Get();

                var response = GetResponseObject<IEnumerable<WidgetModel>>(result);

                Assert.AreEqual(warnings.ExceptionWarningLevel, response.Single(r => r.Name == "Exceptions").WarningLevel);
                Assert.AreEqual(warnings.AssignedWarningLevel, response.Single(r => r.Name == "Assigned").WarningLevel);
                Assert.AreEqual(warnings.NotificationsWarningLevel, response.SingleOrDefault(r => r.Name == "Notifications").WarningLevel);
                Assert.AreEqual(warnings.OutstandingWarningLevel, response.SingleOrDefault(r => r.Name == "Outstanding").WarningLevel);
            }
        }
    }
}