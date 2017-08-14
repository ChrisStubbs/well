namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Domain.ValueObjects;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class AmendmentServiceTests
    {
        private Mock<IAmendmentRepository> amendmentRepository;
        private Mock<IAmendmentFactory> amendmentFactory;
        private Mock<IExceptionEventRepository> exceptionRepository;
        private IAmendmentService service;

        [SetUp]
        public void SetUp()
        {
            amendmentRepository = new Mock<IAmendmentRepository>(MockBehavior.Strict);
            amendmentFactory = new Mock<IAmendmentFactory>(MockBehavior.Strict);
            exceptionRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            service = new AmendmentService(amendmentRepository.Object, amendmentFactory.Object, exceptionRepository.Object);
        }

        public class TheProcessAmendmentsMethod : AmendmentServiceTests
        {
            [Test]
            public void ShouldCallMethodCorrectly()
            {
                var amends = new List<Amendment>
                {
                    new Amendment
                    {
                        JobId = 1,
                        AccountNumber = "1000.123",
                        AmenderName = "Amanda Mender",
                        BranchId = 2,
                        InvoiceNumber = "1232466",
                        AmendmentLines = new List<AmendmentLine>
                        {
                            new AmendmentLine
                            {
                                JobId = 1,
                                ProductCode = "989898",
                                DeliveredQuantity = 12,
                                AmendedDeliveredQuantity = 10,
                                ShortTotal = 0,
                                AmendedShortTotal = 1,
                                DamageTotal = 0,
                                AmendedDamageTotal = 1,
                                RejectedTotal = 0,
                                AmendedRejectedTotal = 0
                            }
                        }
                    }
                };

                amendmentRepository.Setup(x => x.GetAmendments(It.IsAny<IEnumerable<int>>())).Returns(amends);
                amendmentFactory.Setup(x => x.Build(It.IsAny<Amendment>())).Returns(It.IsAny<AmendmentTransaction>());
                exceptionRepository.Setup(x => x.InsertAmendmentTransaction(It.IsAny<AmendmentTransaction>()));

                service.ProcessAmendments(It.IsAny<List<int>>());

                amendmentRepository.Verify(x => x.GetAmendments(It.IsAny<IEnumerable<int>>()), Times.Once);
                amendmentFactory.Verify(x => x.Build(It.IsAny<Amendment>()), Times.Once);
                exceptionRepository.Verify(x => x.InsertAmendmentTransaction(It.IsAny<AmendmentTransaction>()),
                    Times.Once);
            }
        }
    }
}
