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
    using PH.Well.Common.Contracts;
    using Repositories.Contracts;
    using Well.Api.Models;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Services.Contracts;

    [TestFixture]
    public class DeliveryLineControllerTests : BaseControllerTests<DeliveryLineController>
    {
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IDeliveryService> deliveryService;
        private Mock<IDeliveryLineToJobDetailMapper> deliveryLineToJobDetailMapper;

        [SetUp]
        public void Setup()
        {
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            deliveryService = new Mock<IDeliveryService>(MockBehavior.Strict);
            this.deliveryLineToJobDetailMapper = new Mock<IDeliveryLineToJobDetailMapper>(MockBehavior.Strict);

            jobDetailRepository.SetupSet(r => r.CurrentUser = It.IsAny<string>());

            Controller = new DeliveryLineController(
                serverErrorResponseHandler.Object,
                jobDetailRepository.Object,
                deliveryService.Object,
                this.deliveryLineToJobDetailMapper.Object);

            SetupController();
        }

        public class TheUpdateMethod : DeliveryLineControllerTests
        {
            // TODO
            /*[Test]
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
                        new DamageModel() {Quantity = 1, JobDetailReasonId = (int)JobDetailReason.AccumulatedDamages},
                        new DamageModel() {Quantity = 3, JobDetailReasonId = (int)JobDetailReason.BookingError}
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

                this.deliveryLineToJobDetailMapper.Setup(x => x.Map(model, It.IsAny<JobDetail>()));

                deliveryService.Setup(r => r.UpdateDeliveryLine(It.IsAny<JobDetail>(), It.IsAny<string>()));

                HttpResponseMessage response = Controller.Update(model);

                deliveryService.Verify(r => r.UpdateDeliveryLine(It.IsAny<JobDetail>(), It.IsAny<string>()), Times.Once);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                this.deliveryLineToJobDetailMapper.Verify(x => x.Map(model, It.IsAny<JobDetail>()), Times.Once);
            }

            [Test]
            public void GivenException_ThenLogsAndHandles()
            {
                var model = new DeliveryLineModel();
                var ex = new ArgumentException();
                jobDetailRepository.Setup(r => r.GetByJobLine(model.JobId, model.LineNo)).Throws(ex);

                this.serverErrorResponseHandler.Setup(
                    x =>
                        x.HandleException(
                            It.IsAny<HttpRequestMessage>(),
                            ex,
                            "An error occured when updating DeliveryLine")).Returns(It.IsAny<HttpResponseMessage>());

                //ACT
                Controller.Update(model);

                serverErrorResponseHandler.Verify(
                    s => s.HandleException(It.IsAny<HttpRequestMessage>(), It.Is<Exception>(e => e == ex), "An error occured when updating DeliveryLine"));
            }*/
        }
    }
}