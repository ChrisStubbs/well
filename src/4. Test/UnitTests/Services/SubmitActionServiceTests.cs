namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class SubmitActionServiceTests
    {

        private Mock<ILogger> logger;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IUserRepository> userRepository;
        private Mock<ILineItemActionRepository> lineItemActionRepository;
        private Mock<IDeliveryLineCreditMapper> deliveryLineCreditMapper;
        private Mock<ICreditTransactionFactory> creditTransactionFactory;
        private Mock<IExceptionEventRepository> exceptionEventRepository;
        private SubmitActionService submitActionService;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>(MockBehavior.Strict);
            userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            lineItemActionRepository = new Mock<ILineItemActionRepository>(MockBehavior.Strict);
            deliveryLineCreditMapper = new Mock<IDeliveryLineCreditMapper>(MockBehavior.Strict);
            creditTransactionFactory = new Mock<ICreditTransactionFactory>(MockBehavior.Strict);
            exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);

            submitActionService = new SubmitActionService(logger.Object,
                                                userNameProvider.Object,
                                                userRepository.Object,
                                                lineItemActionRepository.Object,
                                                deliveryLineCreditMapper.Object,
                                                creditTransactionFactory.Object,
                                                exceptionEventRepository.Object);
        }


        public class TheSubmitActionMethod : SubmitActionServiceTests
        {
            private const string UserName = "Druno Bobson";
            private readonly User user = new User { Id = 555, Name = UserName };

            private LineItemActionSubmitModel[] GetAction()
            {
                return new[]
                {
                    new LineItemActionSubmitModel{ JobId  = 1,BranchId =2, ProductCode = "Prod1",Reason = JobDetailReason.RecallProduct,Source = JobDetailSource.Assembler,Quantity = 5},
                    new LineItemActionSubmitModel{ JobId  = 1,BranchId =2, ProductCode = "Prod2",Reason = JobDetailReason.Administration,Source = JobDetailSource.Delivery,Quantity = 1},
                    new LineItemActionSubmitModel{ JobId  = 2,BranchId =2, ProductCode = "Prod3",Reason = JobDetailReason.AccumulatedDamages,Source = JobDetailSource.Customer,Quantity = 3}
                };
            }

            private DeliveryLineCredit[] GetDeliveryLineCredits()
            {
                return new DeliveryLineCredit[]
                {
                    new DeliveryLineCredit {JobId = 1,Reason = (int) JobDetailReason.RecallProduct,Source = (int) JobDetailSource.Assembler,Quantity = 5,ProductCode ="Prod1"},
                    new DeliveryLineCredit {JobId = 1,Reason = (int) JobDetailReason.Administration,Source = (int) JobDetailSource.Delivery, Quantity = 1,ProductCode ="Prod2"},
                    new DeliveryLineCredit {JobId = 2,Reason = (int) JobDetailReason.AccumulatedDamages,Source = (int) JobDetailSource.Customer,Quantity = 3,ProductCode ="Prod3"},

                };

            }

            [Test]
            public void ShouldCreditJobInAdamForEachJobWhenActionIsCreditAndSaveAsSubmitted()
            {
                var submitAction = new SubmitActionModel { Action = DeliveryAction.Credit, JobIds = new[] { 1, 2 } };
                var userJobs = new List<UserJob> { new UserJob(user.Id, 1), new UserJob(user.Id, 1) };
                var actions = GetAction();
                var deliveryLineCredits = GetDeliveryLineCredits();
                var job1Credits = deliveryLineCredits.Where(x => x.JobId == 1).ToList();
                var job2Credits = deliveryLineCredits.Where(x => x.JobId == 2).ToList();
                var creditTransaction1 = new CreditTransaction();
                var creditTransaction2 = new CreditTransaction();

                userNameProvider.Setup(x => x.GetUserName()).Returns(UserName);
                userRepository.Setup(x => x.GetByIdentity(UserName)).Returns(user);
                userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(userJobs);
                lineItemActionRepository.Setup(x => x.GetLineItemsWithUnsubmittedActions(submitAction.JobIds, DeliveryAction.Credit)).Returns(actions);

                var calls = 0;
                deliveryLineCreditMapper.Setup(x => x.Map(It.IsAny<LineItemActionSubmitModel[]>())).Callback((IEnumerable<LineItemActionSubmitModel> x) =>
                    {
                        calls++;
                        var xArray = x.ToArray();
                        if (calls == 1)
                        {
                            Assert.That(xArray.Count, Is.EqualTo(2));
                            Assert.That(xArray[0], Is.EqualTo(actions[0]));
                            Assert.That(xArray[1], Is.EqualTo(actions[1]));
                        }
                        else
                        {
                            Assert.That(xArray.Count, Is.EqualTo(1));
                            Assert.That(xArray[0], Is.EqualTo(actions[2]));
                        }
                    }
                ).Returns(() => calls == 1 ? job1Credits : job2Credits);

                creditTransactionFactory.Setup(x => x.Build(job1Credits, 2)).Returns(creditTransaction1);
                creditTransactionFactory.Setup(x => x.Build(job2Credits, 2)).Returns(creditTransaction2);

                exceptionEventRepository.Setup(x => x.InsertCreditEventTransaction(creditTransaction1));
                exceptionEventRepository.Setup(x => x.InsertCreditEventTransaction(creditTransaction2));

                lineItemActionRepository.Setup(x => x.Save(It.IsAny<LineItemActionSubmitModel>()));

                var result = submitActionService.SubmitAction(submitAction);

                deliveryLineCreditMapper.Verify(x => x.Map(It.IsAny<LineItemActionSubmitModel[]>()), Times.Exactly(2));
                creditTransactionFactory.Verify(x => x.Build(job1Credits, 2), Times.Once);
                creditTransactionFactory.Verify(x => x.Build(job2Credits, 2), Times.Once);

                exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction1), Times.Once);
                exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction2), Times.Once);

                lineItemActionRepository.Verify(x => x.Save(It.IsAny<LineItemActionSubmitModel>()), Times.Exactly(3));
                lineItemActionRepository.Verify(x => x.Save(actions[0]), Times.Once);
                Assert.That(actions[0].SubmittedDate,Is.Not.Null);
                Assert.That(actions[0].SubmittedDate.Value, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now));
                lineItemActionRepository.Verify(x => x.Save(actions[1]), Times.Once);
                Assert.That(actions[1].SubmittedDate, Is.Not.Null);
                Assert.That(actions[1].SubmittedDate.Value, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now));
                lineItemActionRepository.Verify(x => x.Save(actions[2]), Times.Once);
                Assert.That(actions[2].SubmittedDate, Is.Not.Null);
                Assert.That(actions[2].SubmittedDate.Value, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now));

            }
        }
    }
}