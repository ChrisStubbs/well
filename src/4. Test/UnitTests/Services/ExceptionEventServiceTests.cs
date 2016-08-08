namespace PH.Well.UnitTests.Services
{
    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;

    [TestFixture]
    public class ExceptionEventServiceTests
    {
        private Mock<IAdamRepository> adamRepository;

        private Mock<IExceptionEventRepository> exceptionEventRepository;

        private ExceptionEventService service;

        [SetUp]
        public void Setup()
        {
            this.adamRepository = new Mock<IAdamRepository>(MockBehavior.Strict);
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);

            this.service = new ExceptionEventService(this.adamRepository.Object, this.exceptionEventRepository.Object);
        }

        public class TheCreditMethod : ExceptionEventServiceTests
        {
            [Test]
            public void ShouldCreditTheInvoice()
            {
                var username = "foo";
                var creditEvent = new CreditEvent { BranchId = 1, InvoiceNumber = "322111.001" };
                var adamSettings = new AdamSettings();

                this.adamRepository.Setup(x => x.CreditInvoice(creditEvent, adamSettings))
                    .Returns(AdamResponse.Success);

                var response = this.service.Credit(creditEvent, adamSettings, username);

                Assert.That(response, Is.EqualTo(AdamResponse.Success));

                this.adamRepository.Verify(x => x.CreditInvoice(creditEvent, adamSettings), Times.Once);
            }

            public void ShouldSaveTheEventForFurtherProcessingIfAdamIsDown()
            {
                var username = "foo";
                var creditEvent = new CreditEvent { BranchId = 1, InvoiceNumber = "322111.001" };
                var adamSettings = new AdamSettings();

                this.adamRepository.Setup(x => x.CreditInvoice(creditEvent, adamSettings))
                    .Returns(AdamResponse.AdamDown);

                this.exceptionEventRepository.Setup(x => x.InsertCreditEvent(creditEvent, username));

                var response = this.service.Credit(creditEvent, adamSettings, username);

                Assert.That(response, Is.EqualTo(AdamResponse.AdamDown));

                this.adamRepository.Verify(x => x.CreditInvoice(creditEvent, adamSettings), Times.Once);
                this.exceptionEventRepository.Verify(x => x.InsertCreditEvent(creditEvent, username), Times.Once);
            }
        }
    }
}