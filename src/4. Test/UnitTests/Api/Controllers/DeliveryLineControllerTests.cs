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

    [TestFixture]
    public class DeliveryLineControllerTests : BaseControllerTests<DeliveryLineController>
    {
        private Mock<ILogger> logger;
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;
        private Mock<IJobDetailRepository> jobDetailRepository;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>(MockBehavior.Strict);
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);

            Controller = new DeliveryLineController(logger.Object,
                serverErrorResponseHandler.Object,
                jobDetailRepository.Object);
            SetupController();
        }

        public class TheUpdateMethod : DeliveryLineControllerTests
        {
            [Test]
            public void HasPutAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Update(null));

                var routeAttribute = GetAttributes<HttpPutAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
            }

            [Test]
            public void HasCorrectRouteAttribute()
            {
                MethodInfo controllerMethod = GetMethod(c => c.Update(null));

                var routeAttribute = GetAttributes<RouteAttribute>(controllerMethod).FirstOrDefault();
                Assert.IsNotNull(routeAttribute);
                Assert.AreEqual("delivery-line", routeAttribute.Template);
            }

            [Test]
            public void GivenNoMatchingLine_ThenReturnsBadRequest()
            {
                var model = new DeliveryLineUpdateModel()
                {
                    JobId = 1,
                    LineNumber = 2
                };

                jobDetailRepository.Setup(r => r.GetByJobLine(model.JobId, model.LineNumber)).Returns((JobDetail) null);

                HttpResponseMessage response = Controller.Update(model);
                var responseModel = GetResponseObject<ErrorModel>(response);

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

                Assert.AreEqual("Unable to update delivery line", responseModel.Message);
                Assert.AreEqual(
                    $"No matching delivery line found for JobId: {model.JobId}, LineNumber: {model.LineNumber}.",
                    responseModel.Errors[0]);
            }

            [Test]
            public void GivenMatchingLine_ThenUpdatesJobDetailAndReturnsOK()
            {
                var model = new DeliveryLineUpdateModel()
                {
                    JobId = 1,
                    LineNumber = 2,
                    Damages = new List<DamageModel>()
                    {
                        new DamageModel() {Quantity = 1, ReasonCode = "CAR01"},
                        new DamageModel() {Quantity = 3, ReasonCode = "CAR02"}
                    },
                    ShortQuantity = 2
                };

                jobDetailRepository.Setup(r => r.GetByJobLine(model.JobId, model.LineNumber)).Returns(new JobDetail()
                {
                    JobId = model.JobId,
                    LineNumber = model.LineNumber,
                    ShortQty = 0
                });

                jobDetailRepository.Setup(r => r.CreateOrUpdate(It.IsAny<JobDetail>()));
                jobDetailRepository.Setup(r => r.CreateOrUpdateJobDetailDamage(It.IsAny<JobDetailDamage>()));

                HttpResponseMessage response = Controller.Update(model);

                jobDetailRepository.Verify(
                    r => r.CreateOrUpdate(It.Is<JobDetail>(j => j.JobId == model.JobId &&
                                                                j.LineNumber == model.LineNumber &&
                                                                j.ShortQty == model.ShortQuantity)), Times.Once);

                jobDetailRepository.Verify(r => r.CreateOrUpdateJobDetailDamage(It.IsAny<JobDetailDamage>()),
                    Times.Exactly(2));

                jobDetailRepository.Verify(r => r.CreateOrUpdateJobDetailDamage(
                    It.Is<JobDetailDamage>(j => j.Qty == 1 && j.Reason == DamageReasons.CAR01)), Times.Once);

                jobDetailRepository.Verify(r => r.CreateOrUpdateJobDetailDamage(
                    It.Is<JobDetailDamage>(j => j.Qty == 3 && j.Reason == DamageReasons.CAR02)), Times.Once);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }

            [Test]
            public void GivenException_ThenLogsAndHandles()
            {
                var model = new DeliveryLineUpdateModel();
                var ex = new ArgumentException();
                jobDetailRepository.Setup(r => r.GetByJobLine(model.JobId, model.LineNumber)).Throws(ex);

                logger.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()));
                serverErrorResponseHandler.Setup(
                    s => s.HandleException(It.IsAny<HttpRequestMessage>(), It.IsAny<Exception>()))
                    .Returns(new HttpResponseMessage());

                //ACT
                Controller.Update(model);

                logger.Verify(l => l.LogError(
                    It.Is<string>(s => s == "An error occcured when updating DeliveryLine"),
                    It.Is<Exception>(e => e == ex)));

                serverErrorResponseHandler.Verify(
                    s => s.HandleException(It.IsAny<HttpRequestMessage>(), It.Is<Exception>(e => e == ex)));
            }
        }
    }
}