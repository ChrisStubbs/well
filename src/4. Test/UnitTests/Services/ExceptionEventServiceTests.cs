﻿namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.UnitTests.Factories;
    using Well.Services.Contracts;

    [TestFixture]
    public class ExceptionEventServiceTests
    {
        private Mock<IAdamRepository> adamRepository;

        private Mock<IExceptionEventRepository> exceptionEventRepository;

        private Mock<IJobRepository> jobRepository;

        private Mock<IUserRepository> userRepository;

        private ExceptionEventService service;

        private Mock<ICreditEventTransactionFactory> creditTransactionFactory;

        [SetUp]
        public void Setup()
        {
            this.adamRepository = new Mock<IAdamRepository>(MockBehavior.Strict);
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.creditTransactionFactory = new Mock<ICreditEventTransactionFactory>(MockBehavior.Strict);

            this.service = new ExceptionEventService(this.adamRepository.Object, this.exceptionEventRepository.Object, this.jobRepository.Object, this.userRepository.Object, this.creditTransactionFactory.Object);
        }

        public class TheCreditMethod : ExceptionEventServiceTests
        {
            [Test]
            public void ShouldCreditTheInvoice()
            {
                var username = "foo";
                var lineDictionary = new Dictionary<int, string>();
                var line = "jhgkjhgkj";
                lineDictionary.Add(1, line);
                var creditEventTransaction = new CreditEventTransaction { BranchId = 22, HeaderSql = "20011.110", LineSql = lineDictionary };

                var creditEvent = new CreditEvent { BranchId = 1, InvoiceNumber = "322111.001", Id = 101 };
                var adamSettings = new AdamSettings();

                this.creditTransactionFactory.Setup(x => x.BuildCreditEventTransaction(creditEvent, username)).Returns(creditEventTransaction);
                this.adamRepository.Setup(x => x.Credit(creditEventTransaction, adamSettings, username))
                    .Returns(AdamResponse.Success);

                this.jobRepository.Setup(x => x.ResolveJobAndJobDetails(creditEvent.Id));
                this.exceptionEventRepository.Setup(x => x.RemovedPendingCredit(creditEvent.InvoiceNumber));
                this.userRepository.Setup(x => x.UnAssignJobToUser(creditEvent.Id));

                var response = this.service.Credit(creditEvent, adamSettings, username);

                Assert.That(response, Is.EqualTo(AdamResponse.Success));

                this.creditTransactionFactory.Verify(x => x.BuildCreditEventTransaction(creditEvent, username), Times.Once);
                this.adamRepository.Verify(x => x.Credit(creditEventTransaction, adamSettings, username), Times.Once);

                this.jobRepository.Verify(x => x.ResolveJobAndJobDetails(creditEvent.Id), Times.Once);
                this.exceptionEventRepository.Verify(x => x.RemovedPendingCredit(creditEvent.InvoiceNumber), Times.Once);
            }

            [Test]
            public void ShouldSaveTheEventForFurtherProcessingIfAdamIsDown()
            {
                var username = "foo";

                var lineDictionary = new Dictionary<int, string>();
                var line = "jhgkjhgkj";
                lineDictionary.Add(1, line);
                var creditEventTransaction = new CreditEventTransaction { BranchId = 22, HeaderSql = "20011.110", LineSql = lineDictionary };

                var creditEvent = new CreditEvent { BranchId = 1, InvoiceNumber = "322111.001" };
                var adamSettings = new AdamSettings();

                this.creditTransactionFactory.Setup(x => x.BuildCreditEventTransaction(creditEvent, username)).Returns(creditEventTransaction);
                this.adamRepository.Setup(x => x.Credit(creditEventTransaction, adamSettings, username))
                    // .Returns(AdamResponse.AdamDown);
                    .Returns(AdamResponse.PartProcessed);

                this.exceptionEventRepository.SetupSet(x => x.CurrentUser = username);

              //  this.exceptionEventRepository.Setup(x => x.InsertCreditEventTransaction(creditEventTransaction));

                var response = this.service.Credit(creditEvent, adamSettings, username);

                //Assert.That(response, Is.EqualTo(AdamResponse.AdamDown));
                Assert.That(response, Is.EqualTo(AdamResponse.PartProcessed));

                this.adamRepository.Verify(x => x.Credit(creditEventTransaction, adamSettings, username), Times.Once);
              //  this.exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditEventTransaction), Times.Once);
            }
        }

        public class TheBulkCreditMethod : ExceptionEventServiceTests
        {
            //[Test]
            //public void ShouldBulkCredit()
            //{
            //    var creditEvents = new List<CreditEvent>
            //    {
            //        CreditEventFactory.New.Build(),
            //        CreditEventFactory.New.Build()
            //    };

            //    this.adamRepository.Setup(x => x.Credit(creditEvents[0], It.IsAny<AdamSettings>(), It.IsAny<string>()))
            //        .Returns(AdamResponse.Success);

            //    this.adamRepository.Setup(x => x.Credit(creditEvents[1], It.IsAny<AdamSettings>(), It.IsAny<string>()))
            //        .Returns(AdamResponse.Success);

            //    this.jobRepository.Setup(x => x.ResolveJobAndJobDetails(creditEvents[0].Id));
            //    this.exceptionEventRepository.Setup(x => x.RemovedPendingCredit(creditEvents[0].InvoiceNumber));
            //    this.userRepository.Setup(x => x.UnAssignJobToUser(creditEvents[0].Id));
            //    var response = this.service.BulkCredit(creditEvents, "jonny the foo");

            //    this.adamRepository.Verify(x => x.Credit(creditEvents[0], It.IsAny<AdamSettings>(), It.IsAny<string>()), Times.Once);

            //    this.adamRepository.Verify(x => x.Credit(creditEvents[1], It.IsAny<AdamSettings>(), It.IsAny<string>()), Times.Once);

            //    this.jobRepository.Verify(x => x.ResolveJobAndJobDetails(creditEvents[0].Id), Times.Exactly(2));
            //    this.exceptionEventRepository.Verify(x => x.RemovedPendingCredit(creditEvents[0].InvoiceNumber), Times.Exactly(2));

            //    Assert.That(response, Is.EqualTo(AdamResponse.Success));
            //}

            //[Test]
            //public void ShouldCreateCreditEventWhenAdamIsDown()
            //{
            //    var creditEvents = new List<CreditEvent>
            //    {
            //        CreditEventFactory.New.Build()
            //    };

            //    var username = "foo";

            //    this.adamRepository.Setup(x => x.Credit(creditEvents[0], It.IsAny<AdamSettings>(), It.IsAny<string>()))
            //        .Returns(AdamResponse.AdamDown);

            //    this.exceptionEventRepository.SetupSet(x => x.CurrentUser = username);
            //    this.exceptionEventRepository.Setup(x => x.InsertCreditEvent(creditEvents[0]));

            //    var response = this.service.BulkCredit(creditEvents, username);

            //    this.adamRepository.Verify(x => x.Credit(creditEvents[0], It.IsAny<AdamSettings>(), It.IsAny<string>()), Times.Once);

            //    this.exceptionEventRepository.Verify(x => x.InsertCreditEvent(creditEvents[0]), Times.Once);

            //    Assert.That(response, Is.EqualTo(AdamResponse.AdamDown));
            //}
        }
    }
}