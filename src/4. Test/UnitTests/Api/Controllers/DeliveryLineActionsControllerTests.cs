namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        private Mock<ILogger> logger;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IDeliveryService> deliveryService;

        [SetUp]
        public void Setup()
        {
            serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            logger = new Mock<ILogger>(MockBehavior.Strict);
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            deliveryService = new Mock<IDeliveryService>(MockBehavior.Strict);

            jobDetailRepository.SetupSet(r => r.CurrentUser = It.IsAny<string>());

            Controller = new DeliveryLineActionsController(
                serverErrorResponseHandler.Object,
                logger.Object,
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

            [Test]
            public void GivenNoMatchingLine_ThenReturnsBadRequest()
            {
                var model = new DeliveryLineActionsModel()
                {
                    JobDetailId = 1
                };

                jobDetailRepository.Setup(r => r.GetById(model.JobDetailId)).Returns((JobDetail) null);

                logger.Setup(l => l.LogError(It.IsAny<string>()));

                HttpResponseMessage response = Controller.Post(model);
                var responseModel = GetResponseObject<ErrorModel>(response);

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

                Assert.AreEqual("Unable to update delivery line actions", responseModel.Message);
                Assert.AreEqual($"No matching delivery line found for JobDetailId: {model.JobDetailId}.",
                    responseModel.Errors[0]);

                var expectedString =
                    $"Unable to update delivery line actions. No matching delivery line found for JobDetailId: {model.JobDetailId}.";
                logger.Verify(l => l.LogError(It.Is<string>(s => s == expectedString)));
            }

            [Test]
            public void GivenException_ThenLogsAndHandles()
            {
                var model = new DeliveryLineActionsModel();
                var ex = new ArgumentException();
                jobDetailRepository.Setup(r => r.GetById(model.JobDetailId)).Throws(ex);

                this.serverErrorResponseHandler.Setup(
                        x => x.HandleException(It.IsAny<HttpRequestMessage>(), ex, It.IsAny<string>()))
                    .Returns(It.IsAny<HttpResponseMessage>());

                //ACT
                Controller.Post(model);

                serverErrorResponseHandler.Verify(s =>
                    s.HandleException(It.IsAny<HttpRequestMessage>(), It.Is<Exception>(e => e == ex),
                        "An error occured when updating DeliveryLine Actions"));
            }

            [Test]
            public void GivenNewAndAmendedDraftAction_ThenChangesSaved()
            {
                var model = new DeliveryLineActionsModel()
                {
                    JobDetailId = 1,
                    DraftActions = new List<ActionModel>()
                    {
                        new ActionModel() { Action = ExceptionAction.CreditAndReorder, Quantity = 2, Status = ActionStatus.Draft},
                         new ActionModel() { Action = ExceptionAction.Credit, Quantity = 1, Status = ActionStatus.Draft}
                    }
                };

                jobDetailRepository.Setup(r => r.GetById(model.JobDetailId)).Returns(new JobDetail()
                {
                    Actions = new List<JobDetailAction>(new List<JobDetailAction>()
                    {
                        new JobDetailAction()
                        {
                            JobDetailId = 1,
                            Action = ExceptionAction.Reject,
                            Quantity = 1,
                            Status = ActionStatus.Submitted
                        },
                        new JobDetailAction()
                        {
                            JobDetailId = 1,
                            Action = ExceptionAction.CreditAndReorder,
                            Quantity = 1,
                            Status = ActionStatus.Draft
                        }
                    })
                });

                deliveryService.Setup(d => d.UpdateDraftActions(It.IsAny<JobDetail>(), ""));

                HttpResponseMessage response = Controller.Post(model);

                deliveryService.Verify(d => d.UpdateDraftActions(It.Is<JobDetail>(j =>
                    j.Actions[0].JobDetailId == model.JobDetailId &&
                    j.Actions[0].Action == ExceptionAction.Reject &&
                    j.Actions[0].Quantity == 1 &&
                    j.Actions[0].Status == ActionStatus.Submitted), ""));

                deliveryService.Verify(d => d.UpdateDraftActions(It.Is<JobDetail>(j =>
                    j.Actions[1].JobDetailId == model.JobDetailId &&
                    j.Actions[1].Action == ExceptionAction.CreditAndReorder &&
                    j.Actions[1].Quantity == 2 &&
                    j.Actions[1].Status == ActionStatus.Draft), ""));

                deliveryService.Verify(d => d.UpdateDraftActions(It.Is<JobDetail>(j =>
                    j.Actions[2].JobDetailId == model.JobDetailId &&
                    j.Actions[2].Action == ExceptionAction.Credit &&
                    j.Actions[2].Quantity == 1 &&
                    j.Actions[2].Status == ActionStatus.Draft), ""));

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}