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
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;
    using Well.Domain;
    using Well.Services.Contracts;

    [TestFixture]
    public class DeliveryControllerTests : BaseControllerTests<DeliveryController>
    {
        private Mock<IDeliveryReadRepository> deliveryReadRepository;
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;
        private Mock<IDeliveryToDetailMapper> deliveryToDetailMapper;
        private Mock<IDeliveryService> deliveryService;
        private Mock<IExceptionEventRepository> exceptionEventRepository;
        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Loose);
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            deliveryToDetailMapper = new Mock<IDeliveryToDetailMapper>(MockBehavior.Strict);
            deliveryService = new Mock<IDeliveryService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("user");

            this.Controller = new DeliveryController(
                this.deliveryReadRepository.Object,
                this.serverErrorResponseHandler.Object,
                this.deliveryToDetailMapper.Object,
                this.deliveryService.Object,
                this.userNameProvider.Object);

            this.SetupController();
        }

        public class TheGetExceptionMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetExceptions()
            {
                var deliveries = new List<Delivery> {DeliveryFactory.New.Build()};

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries(It.IsAny<string>(), false)).Returns(deliveries);

                var response = this.Controller.GetExceptions();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedDeliveries = new List<Delivery>();

                response.TryGetContentValue(out returnedDeliveries);

                Assert.That(returnedDeliveries.Count, Is.EqualTo(1));
                Assert.That(returnedDeliveries[0].Assigned, Is.EqualTo(deliveries[0].Assigned));
            }

            [Test]
            public void ShouldReturnNotFoundWhenNoExceptions()
            {
                var deliveries = new List<Delivery>();

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries("", false)).Returns(deliveries);

                var response = this.Controller.GetExceptions();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries(It.IsAny<string>(), false))
                    .Throws(exception);

                this.serverErrorResponseHandler.Setup(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting exceptions")).Returns(It.IsAny<HttpResponseMessage>());

                var response = this.Controller.GetExceptions();

                this.serverErrorResponseHandler.Verify(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting exceptions"), Times.Once);
            }
        }

        public class TheGetCleanMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetCleanDeliveries()
            {
                var deliveries = new List<Delivery> {DeliveryFactory.New.Build()};

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries(It.IsAny<string>())).Returns(deliveries);

                var response = this.Controller.GetCleanDeliveries();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedDeliveries = new List<Delivery>();

                response.TryGetContentValue(out returnedDeliveries);

                Assert.That(returnedDeliveries.Count, Is.EqualTo(1));
                Assert.That(returnedDeliveries[0].Assigned, Is.EqualTo(deliveries[0].Assigned));
            }

            [Test]
            public void ShouldReturnNotFoundWhenNoCleanDeliveries()
            {
                var deliveries = new List<Delivery>();

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries(It.IsAny<string>())).Returns(deliveries);

                var response = this.Controller.GetCleanDeliveries();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries(It.IsAny<string>())).Throws(exception);

                this.serverErrorResponseHandler.Setup(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting clean deliveries")).Returns(It.IsAny<HttpResponseMessage>());

                var response = this.Controller.GetCleanDeliveries();

                this.serverErrorResponseHandler.Verify(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting clean deliveries"), Times.Once);
            }
        }

        public class TheGetResolvedDeliveriesMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetResolvedDeliveries()
            {
                var deliveries = new List<Delivery> { DeliveryFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetResolvedDeliveries(It.IsAny<string>())).Returns(deliveries);

                var response = this.Controller.GetResolvedDeliveries();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedDeliveries = new List<Delivery>();

                response.TryGetContentValue(out returnedDeliveries);

                Assert.That(returnedDeliveries.Count, Is.EqualTo(1));
                Assert.That(returnedDeliveries[0].Assigned, Is.EqualTo(deliveries[0].Assigned));
            }

            [Test]
            public void ShouldReturnNotFoundWhenNoResolvedDeliveries()
            {
                var deliveries = new List<Delivery>();

                this.deliveryReadRepository.Setup(x => x.GetResolvedDeliveries(It.IsAny<string>())).Returns(deliveries);

                var response = this.Controller.GetResolvedDeliveries();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetResolvedDeliveries(It.IsAny<string>())).Throws(exception);

                this.serverErrorResponseHandler.Setup(
                        x =>
                            x.HandleException(
                                It.IsAny<HttpRequestMessage>(),
                                exception,
                                "An error occured when getting resolved deliveries"))
                    .Returns(It.IsAny<HttpResponseMessage>());

                var response = this.Controller.GetResolvedDeliveries();

                this.serverErrorResponseHandler.Verify(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting resolved deliveries"), Times.Once);
            }
        }

        public class TheGetDeliveryByIdMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetTheDeliveryById()
            {
                var delivery = DeliveryDetailFactory.New.Build();
                var deliveryId = 4;

                var lines = new List<DeliveryLine> {DeliveryLineFactory.New.Build()};

                this.deliveryReadRepository.Setup(x => x.GetDeliveryById(deliveryId, It.IsAny<string>()))
                    .Returns(delivery);
                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(deliveryId)).Returns(lines);
                this.deliveryToDetailMapper.Setup(x => x.Map(lines, delivery))
                    .Returns(new DeliveryDetailModel {AccountCode = "1000"});

                var response = this.Controller.GetDelivery(deliveryId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ShouldReturnNotFoundWhenNoDeliveries()
            {
                var delivery = DeliveryDetailFactory.New.With(x => x.AccountCode = null).Build();
                var deliveryId = 4;

                var lines = new List<DeliveryLine> {DeliveryLineFactory.New.Build()};

                this.deliveryReadRepository.Setup(x => x.GetDeliveryById(deliveryId, It.IsAny<string>()))
                    .Returns(delivery);
                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(deliveryId)).Returns(lines);

                this.deliveryToDetailMapper.Setup(x => x.Map(lines, delivery))
                    .Returns(new DeliveryDetailModel {AccountCode = ""});

                var response = this.Controller.GetDelivery(deliveryId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();
                var deliveryId = 4;

                this.deliveryReadRepository.Setup(x => x.GetDeliveryById(deliveryId, It.IsAny<string>()))
                    .Throws(exception);

                this.serverErrorResponseHandler.Setup(
                        x =>
                            x.HandleException(
                                It.IsAny<HttpRequestMessage>(),
                                exception,
                                "An error occured when getting delivery detail id: 4"))
                    .Returns(It.IsAny<HttpResponseMessage>());

                var response = this.Controller.GetDelivery(deliveryId);

                this.serverErrorResponseHandler.Verify(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting delivery detail id: 4"), Times.Once);
            }
        }

        public class TheGetApprovalsMethod : DeliveryControllerTests
        {
            [Test]
            public void HasGetAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.GetApprovals());
                var routeAttribute = GetAttributes<HttpGetAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
            }

            [Test]
            public void HasCorrectRoute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.GetApprovals());
                var routeAttribute = GetAttributes<RouteAttribute>(controllerMethod).FirstOrDefault();
                Assert.AreEqual("deliveries/approval", routeAttribute.Template);
            }

            [Test]
            public void GivenNoApprovals_ThenReturnsNotFound()
            {
                deliveryService.Setup(d => d.GetApprovals(It.IsAny<string>())).Returns(new List<Delivery>());

                HttpResponseMessage response = Controller.GetApprovals();

                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Test]
            public void GivenApprovals_ThenReturnsListWithOKStatus()
            {
                var expectedDeliveries = new List<Delivery>()
                {
                    new Delivery() {Id = 1}
                };
                deliveryService.Setup(d => d.GetApprovals(It.IsAny<string>())).Returns(expectedDeliveries);

                HttpResponseMessage response = Controller.GetApprovals();
                var responseModel = GetResponseObject<List<Delivery>>(response);


                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(expectedDeliveries[0].Id, responseModel[0].Id);
            }
        }

        public class TheSaveGrnMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldSaveTheGrnAgainstTheJob()
            {
                var model = new GrnModel {Id = 302, GrnNumber = "123212", BranchId = 2};

                this.deliveryService.Setup(
                    x => x.SaveGrn(model.Id, model.GrnNumber, model.BranchId, It.IsAny<string>()));

                //ACT
                var response = this.Controller.SaveGrn(model);

                deliveryService.Verify(d => d.SaveGrn(model.Id, model.GrnNumber, model.BranchId, It.IsAny<string>()), Times.Once);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            }
        }

    }
}