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
        private Mock<IUserNameProvider> userNameProvider;
        private string userIdentity = "bob";
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
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("bob");

            notificationsRepository = new Mock<INotificationRepository>(MockBehavior.Strict);
            deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);

            this.Controller = new WidgetController(serverErrorResponseHandler.Object,
                this.logger.Object,
                this.userStatsRepository.Object,
                this.widgetRepository.Object,
                this.mapper.Object,
                this.validator.Object,
                this.userNameProvider.Object,
                this.notificationsRepository.Object,
                this.deliveryReadRepository.Object);
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
            public void ReturnsCorrectWarningLevels()
            {
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

            [Test]
            public void ReturnsCorrectCounts()
            {
                string userIdentity = "bob";
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userIdentity), new[] { "A role" });

                var exceptions = new List<Delivery>();
                exceptions.Add(new Delivery() {IsPendingCredit = false, IdentityName = "jim", DeliveryDate = DateTime.Now});
                exceptions.Add(new Delivery() {IsPendingCredit = false, IdentityName = "jim", DeliveryDate = DateTime.Now.AddDays(-1)});
                exceptions.Add(new Delivery() {IsPendingCredit = false, IdentityName = "bob", DeliveryDate = DateTime.Now});
                exceptions.Add(new Delivery() {IsPendingCredit = false, IdentityName = "bob", DeliveryDate = DateTime.Now.AddDays(-1)});
                exceptions.Add(new Delivery() {IsPendingCredit = false, IdentityName = "bob", DeliveryDate = DateTime.Now.AddDays(-1)});
                exceptions.Add(new Delivery() {IsPendingCredit = false, IdentityName = "jim", DeliveryDate = DateTime.Now.AddDays(-1)});
                exceptions.Add(new Delivery() {IsPendingCredit = true, IdentityName = "jim", DeliveryDate = DateTime.Now});
                exceptions.Add(new Delivery() {IsPendingCredit = true, IdentityName = "jim", DeliveryDate = DateTime.Now.AddDays(-1)});
                exceptions.Add(new Delivery() {IsPendingCredit = true, IdentityName = "jim", DeliveryDate = DateTime.Now});
                exceptions.Add(new Delivery() {IsPendingCredit = true, IdentityName = "bob", DeliveryDate = DateTime.Now.AddDays(-1)});

                deliveryReadRepository.Setup(d => d.GetExceptionDeliveries(userIdentity, true)).Returns(exceptions);
                notificationsRepository.Setup(n => n.GetNotifications()).Returns(new List<Notification>() {new Notification()});

                this.userStatsRepository.Setup(r => r.GetWidgetWarningLevels(userIdentity)).Returns(new WidgetWarningLevels());

                var result = Controller.Get();

                var response = GetResponseObject<IEnumerable<WidgetModel>>(result);

                Assert.AreEqual(6, response.Single(r => r.Name == "Exceptions").Links.Single(l => l.CountName == "unsubmitted-exceptions").Count);
                Assert.AreEqual(4, response.Single(r => r.Name == "Exceptions").Links.Single(l => l.CountName == "approval-exceptions").Count);
                Assert.AreEqual(3, response.Single(r => r.Name == "Assigned").Links.Single(l => l.CountName == "my-unsubmitted-exceptions").Count);
                Assert.AreEqual(1, response.Single(r => r.Name == "Assigned").Links.Single(l => l.CountName == "my-approval-exceptions").Count);
                Assert.AreEqual(4, response.Single(r => r.Name == "Outstanding").Links.Single(l => l.CountName == "outstanding-unsubmitted-exceptions").Count);
                Assert.AreEqual(2, response.Single(r => r.Name == "Outstanding").Links.Single(l => l.CountName == "outstanding-approval-exceptions").Count);
                Assert.AreEqual(1, response.SingleOrDefault(r => r.Name == "Notifications").Count);
            }
        }
    }
}