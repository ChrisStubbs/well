namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class DeliveryControllerTests : BaseControllerTests<DeliveryController>
    {
        private Mock<IDeliveryReadRepository> deliveryReadRepository;

        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;

        private Mock<IDeliveryToDetailMapper> deliveryToDetailMapper;

        [SetUp]
        public void Setup()
        {
            this.deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);
            this.serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            this.deliveryToDetailMapper = new Mock<IDeliveryToDetailMapper>(MockBehavior.Strict);

            this.Controller = new DeliveryController(
                this.deliveryReadRepository.Object, 
                this.serverErrorResponseHandler.Object,
                this.deliveryToDetailMapper.Object);

            this.SetupController();
        }

        public class TheGetExceptionMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetExceptions()
            {
                var deliveries = new List<Delivery> { DeliveryFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries("")).Returns(deliveries);

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

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries("")).Returns(deliveries);

                var response = this.Controller.GetExceptions();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries("")).Throws(exception);

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

        public class TheGetExceptionByRouteIdMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetExceptionsFilteredByRouteNumber()
            {
                var deliveries = new List<Delivery>
                {
                    DeliveryFactory.New.With(x => x.RouteNumber = "011").Build(),
                    DeliveryFactory.New.With(x => x.RouteNumber = "011").Build(),
                    DeliveryFactory.New.With(x => x.RouteNumber = "02").Build()
                };

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries("")).Returns(deliveries);

                var response = this.Controller.GetExceptions("011");

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedDeliveries = new List<Delivery>();

                response.TryGetContentValue(out returnedDeliveries);

                Assert.That(returnedDeliveries.Count, Is.EqualTo(2));
                Assert.That(returnedDeliveries[0].RouteNumber, Is.EqualTo(deliveries[0].RouteNumber));
                Assert.That(returnedDeliveries[1].RouteNumber, Is.EqualTo(deliveries[1].RouteNumber));
            }

            [Test]
            public void ShouldReturnNotFoundWhenNoExceptions()
            {
                var deliveries = new List<Delivery>();

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries("")).Returns(deliveries);

                var response = this.Controller.GetExceptions("011");

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetExceptionDeliveries("")).Throws(exception);

                this.serverErrorResponseHandler.Setup(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting exceptions for route 011")).Returns(It.IsAny<HttpResponseMessage>());

                var response = this.Controller.GetExceptions("011");

                this.serverErrorResponseHandler.Verify(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting exceptions for route 011"), Times.Once);
            }
        }

        public class TheGetCleanMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetCleanDeliveries()
            {
                var deliveries = new List<Delivery> { DeliveryFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries("")).Returns(deliveries);

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

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries("")).Returns(deliveries);

                var response = this.Controller.GetCleanDeliveries();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries("")).Throws(exception);

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

        public class TheGetCleanDeliveriesByRouteIdMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetCleanDeliveriesFilteredByRouteNumber()
            {
                var deliveries = new List<Delivery>
                {
                    DeliveryFactory.New.With(x => x.RouteNumber = "011").Build(),
                    DeliveryFactory.New.With(x => x.RouteNumber = "011").Build(),
                    DeliveryFactory.New.With(x => x.RouteNumber = "02").Build()
                };

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries("")).Returns(deliveries);

                var response = this.Controller.GetCleanDeliveries("011");

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedDeliveries = new List<Delivery>();

                response.TryGetContentValue(out returnedDeliveries);

                Assert.That(returnedDeliveries.Count, Is.EqualTo(2));
                Assert.That(returnedDeliveries[0].RouteNumber, Is.EqualTo(deliveries[0].RouteNumber));
                Assert.That(returnedDeliveries[1].RouteNumber, Is.EqualTo(deliveries[1].RouteNumber));
            }

            [Test]
            public void ShouldReturnNotFoundWhenNoCleanDeliveries()
            {
                var deliveries = new List<Delivery>();

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries("")).Returns(deliveries);

                var response = this.Controller.GetCleanDeliveries("011");

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetCleanDeliveries("")).Throws(exception);

                this.serverErrorResponseHandler.Setup(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting clean deliveries for route 011")).Returns(It.IsAny<HttpResponseMessage>());

                var response = this.Controller.GetCleanDeliveries("011");

                this.serverErrorResponseHandler.Verify(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting clean deliveries for route 011"), Times.Once);
            }
        }

        public class TheGetResolvedDeliveriesMethod : DeliveryControllerTests
        {
            [Test]
            public void ShouldGetResolvedDeliveries()
            {
                var deliveries = new List<Delivery> { DeliveryFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetResolvedDeliveries("")).Returns(deliveries);

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

                this.deliveryReadRepository.Setup(x => x.GetResolvedDeliveries("")).Returns(deliveries);

                var response = this.Controller.GetResolvedDeliveries();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();

                this.deliveryReadRepository.Setup(x => x.GetResolvedDeliveries("")).Throws(exception);

                this.serverErrorResponseHandler.Setup(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting resolved deliveries")).Returns(It.IsAny<HttpResponseMessage>());

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

                var lines = new List<DeliveryLine> { DeliveryLineFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetDeliveryById(deliveryId, "")).Returns(delivery);
                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(deliveryId)).Returns(lines);
                this.deliveryToDetailMapper.Setup(x => x.Map(lines, delivery)).Returns(new DeliveryDetailModel { AccountCode = "1000" });

                var response = this.Controller.GetDelivery(deliveryId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ShouldReturnNotFoundWhenNoDeliveries()
            {
                var delivery = DeliveryDetailFactory.New.With(x => x.AccountCode = null).Build();
                var deliveryId = 4;

                var lines = new List<DeliveryLine> { DeliveryLineFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetDeliveryById(deliveryId, "")).Returns(delivery);
                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(deliveryId)).Returns(lines);

                this.deliveryToDetailMapper.Setup(x => x.Map(lines, delivery)).Returns(new DeliveryDetailModel { AccountCode = "" });

                var response = this.Controller.GetDelivery(deliveryId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldThrowExceptionWhenUnhandledError()
            {
                var exception = new Exception();
                var deliveryId = 4;

                this.deliveryReadRepository.Setup(x => x.GetDeliveryById(deliveryId, "")).Throws(exception);

                this.serverErrorResponseHandler.Setup(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting delivery detail id: 4")).Returns(It.IsAny<HttpResponseMessage>());

                var response = this.Controller.GetDelivery(deliveryId);

                this.serverErrorResponseHandler.Verify(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            exception,
                            "An error occured when getting delivery detail id: 4"), Times.Once);
            }
        }
    }
}