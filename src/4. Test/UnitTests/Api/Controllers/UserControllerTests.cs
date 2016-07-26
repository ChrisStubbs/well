namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Net.Http;
    using System.Security.Principal;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Hosting;
    using System.Web.Http.Routing;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Services.Contracts;

    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IBranchService> branchService;

        private UserController controller;

        [SetUp]
        public void Setup()
        {
            this.branchService = new Mock<IBranchService>(MockBehavior.Strict);
            this.controller = new UserController(this.branchService.Object);

            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/events");
            var route = config.Routes.MapHttpRoute("Branch", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "Branch" } });

            request.Properties.Add("transactionId", "35AAB0C8-1AD1-401A-AFF9-D31E6926A1BD");
            controller.RequestContext = new HttpRequestContext { Url = new UrlHelper(request) };
            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Url = new UrlHelper(request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties["MS_HttpContext"] = null;
            controller.RequestContext.Principal = new GenericPrincipal(new GenericIdentity("foo"), new[] { "A role" });
        }

        public class TheUserBranchesMethod : UserControllerTests
        {
            [Test]
            public void ShouldReturnTheUserBranchesInformationForDashboardHeader()
            {

            }
        }
    }
}