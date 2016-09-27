namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Web.Http;
    using Moq;
    using NUnit.Framework;
    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;
    using Repositories.Contracts;
    using Well.Api.Models;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Services.Contracts;

    [TestFixture]
    public class DeliveryLineActionsControllerTests : BaseControllerTests<DeliveryLineActionsController>
    {
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IDeliveryService> deliveryService;

        [SetUp]
        public void Setup()
        {
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            deliveryService = new Mock<IDeliveryService>(MockBehavior.Strict);

            jobDetailRepository.SetupSet(r => r.CurrentUser = It.IsAny<string>());

            Controller = new DeliveryLineActionsController(
                serverErrorResponseHandler.Object,
                jobDetailRepository.Object,
                deliveryService.Object);

            SetupController();
        }

        public class ThePostMethod : DeliveryLineActionsControllerTests
        {
            [Test]
            public void HasPostAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Post(null));

                var routeAttribute = GetAttributes<HttpPostAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
            }

            [Test]
            public void HasCorrectRouteAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Post(null));

                var routeAttribute = GetAttributes<RouteAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
                Assert.AreEqual("delivery-line-actions", routeAttribute.Template);
            }
        }
    }
}