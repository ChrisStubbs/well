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
        private Mock<IJobDetailDamageRepo> jobDetailDamageRepo;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>(MockBehavior.Strict);
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            jobDetailDamageRepo = new Mock<IJobDetailDamageRepo>(MockBehavior.Strict);

            jobDetailRepository.SetupSet(r => r.CurrentUser = It.IsAny<string>());
            jobDetailDamageRepo.SetupSet(r => r.CurrentUser = It.IsAny<string>());

            Controller = new DeliveryLineController(logger.Object,
                serverErrorResponseHandler.Object,
                jobDetailRepository.Object,
                jobDetailDamageRepo.Object);
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
                var model = new DeliveryLineModel()
                {
                    JobId = 1,
                    LineNo = 2
                };

                jobDetailRepository.Setup(r => r.GetByJobLine(model.JobId, model.LineNo)).Returns((JobDetail) null);

                HttpResponseMessage response = Controller.Update(model);
                var responseModel = GetResponseObject<ErrorModel>(response);

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

                Assert.AreEqual("Unable to update delivery line", responseModel.Message);
                Assert.AreEqual(
                    $"No matching delivery line found for JobId: {model.JobId}, LineNumber: {model.LineNo}.",
                    responseModel.Errors[0]);
            }

            [Test]
            public void GivenMatchingLine_ThenUpdatesJobDetailAndReturnsOK()
            {
                var model = new DeliveryLineModel()
                {
                    JobId = 1,
                    LineNo = 2,
                    Damages = new List<DamageModel>()
                    {
                        new DamageModel() {Quantity = 1, ReasonCode = "CAR01"},
                        new DamageModel() {Quantity = 3, ReasonCode = "CAR02"}
                    },
                    ShortQuantity = 2
                };

                jobDetailRepository.Setup(r => r.GetByJobLine(model.JobId, model.LineNo)).Returns(new JobDetail()
                {
                    Id = 5,
                    JobId = model.JobId,
                    LineNumber = model.LineNo,
                    ShortQty = 0
                });

                jobDetailRepository.Setup(r => r.Update(It.IsAny<JobDetail>()));
                jobDetailRepository.Setup(r => r.GetById(It.IsAny<int>()));
                jobDetailDamageRepo.Setup(r => r.Delete(It.IsAny<int>()));
                jobDetailDamageRepo.Setup(r => r.Save(It.IsAny<JobDetailDamage>()));


                HttpResponseMessage response = Controller.Update(model);

                jobDetailRepository.Verify(
                    r => r.Update(It.Is<JobDetail>(j => j.JobId == model.JobId &&
                                                                j.LineNumber == model.LineNo &&
                                                                j.ShortQty == model.ShortQuantity)), Times.Once);

                jobDetailDamageRepo.Verify(r => r.Delete(It.Is<int>(i => i == 5)));

                jobDetailDamageRepo.Verify(r => r.Save(It.IsAny<JobDetailDamage>()),
                    Times.Exactly(2));

                jobDetailDamageRepo.Verify(r => r.Save(
                    It.Is<JobDetailDamage>(j => j.JobDetailId == 5 && j.Qty == 1 && j.DamageReason == DamageReasons.CAR01)), Times.Once);

                jobDetailDamageRepo.Verify(r => r.Save(
                    It.Is<JobDetailDamage>(j => j.JobDetailId == 5 && j.Qty == 3 && j.DamageReason == DamageReasons.CAR02)), Times.Once);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }

            [Test]
            public void GivenException_ThenLogsAndHandles()
            {
                var model = new DeliveryLineModel();
                var ex = new ArgumentException();
                jobDetailRepository.Setup(r => r.GetByJobLine(model.JobId, model.LineNo)).Throws(ex);

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