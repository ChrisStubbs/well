namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Net;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;

    [TestFixture]
    public class JobDetailReasonControllerTests : BaseControllerTests<JobDetailReasonController>
    {
        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;
        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            this.serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("user");

            this.Controller = new JobDetailReasonController(this.serverErrorResponseHandler.Object, this.userNameProvider.Object);

            this.SetupController();
        }

        public class TheGetMethod : JobDetailReasonControllerTests
        {
            [Test]
            public void ShouldReturnAllJobDetailReasons()
            {
                var response = this.Controller.Get();

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.That(content, Does.Contain("[{\"id\":0,\"description\":\"Not Defined\"},{\"id\":1,\"description\":\"No Credit\"},{\"id\":2,\"description\":\"Damaged Goods\"},{\"id\":3,\"description\":\"Short Delivered\"},{\"id\":4,\"description\":\"Booking Error\"},{\"id\":5,\"description\":\"Picking Error\"},{\"id\":7,\"description\":\"Administration\"},{\"id\":10,\"description\":\"Customer Damaged\"},{\"id\":11,\"description\":\"Short Dated\"},{\"id\":13,\"description\":\"Signed Short\"},{\"id\":14,\"description\":\"Out Of Date Stock\"},{\"id\":15,\"description\":\"Short T.B.A.\"},{\"id\":17,\"description\":\"Freezer Chiller Breakdown\"},{\"id\":18,\"description\":\"Not Enough Room\"},{\"id\":19,\"description\":\"Out Of Temp\"},{\"id\":20,\"description\":\"Duplicate Order\"},{\"id\":21,\"description\":\"Not Ordered\"},{\"id\":22,\"description\":\"Shop Closed No Staff\"}]"));

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }
    }
}