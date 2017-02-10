namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class PendingCreditControllerTests : BaseControllerTests<PendingCreditController>
    {
        private Mock<IDeliveryReadRepository> deliveryReadRepository;

        [SetUp]
        public void Setup()
        {
            this.deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);

            this.Controller = new PendingCreditController(this.deliveryReadRepository.Object);
            this.SetupController();
        }

        public class TheGetMethod : PendingCreditControllerTests
        {
            [Test]
            public void ShouldGetAllPendingCreditsForUser()
            {
                var pendingCredits = new List<Delivery>();
                pendingCredits.Add(new Delivery { AccountName = "foo" });

                this.deliveryReadRepository.Setup(x => x.GetByPendingCredit(It.IsAny<string>())).Returns(pendingCredits);

                var response = this.Controller.Get();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var pending = new List<Delivery>();

                response.TryGetContentValue(out pending);

                Assert.That(pending.Count, Is.EqualTo(1));
                Assert.That(pending[0].AccountName, Is.EqualTo("foo"));
            }
        }

        public class ThegetDetailsMethod : PendingCreditControllerTests
        {
            [Test]
            public void ShouldGetDetailsForAPendingCredit()
            {
                var details = new List<PendingCreditDetail>();
                details.Add(new PendingCreditDetail { Action = "foo me" });

                var jobId = 998;

                this.deliveryReadRepository.Setup(x => x.GetPendingCreditDetail(jobId))
                    .Returns(details);

                var result = this.Controller.GetDetails(jobId);

                var pending = new List<PendingCreditDetail>();

                result.TryGetContentValue(out pending);

                Assert.That(pending.Count, Is.EqualTo(1));
                Assert.That(pending[0].Action, Is.EqualTo("foo me"));
            }
        }
    }
}