namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Net;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;

    [TestFixture]
    public class JobDetailSourceControllerTests : BaseControllerTests<JobDetailSourceController>
    {
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;

        [SetUp]
        public void Setup()
        {
            this.serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);

            this.Controller = new JobDetailSourceController(this.serverErrorResponseHandler.Object);

            this.SetupController();
        }

        public class TheGetMethod : JobDetailSourceControllerTests
        {
            [Test]
            public void ShouldReturnJobDetailSourceEnumeration()
            {
                var result = this.Controller.Get();

                var content = result.Content.ReadAsStringAsync().Result;

                Assert.That(content, Does.Contain("[{\"id\":0,\"description\":\"Not Defined\"},{\"id\":1,\"description\":\"Input\"},{\"id\":2,\"description\":\"Assembler\"},{\"id\":3,\"description\":\"Checker\"},{\"id\":4,\"description\":\"Packer\"},{\"id\":5,\"description\":\"Confirming\"},{\"id\":6,\"description\":\"Delivery\"},{\"id\":7,\"description\":\"Rep Telesales\"},{\"id\":8,\"description\":\"Product Fault\"},{\"id\":9,\"description\":\"Customer\"}]"));

                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }
    }
}