namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Common;
    using Well.Common.Contracts;

    [TestFixture]
    public class BaseDapperProxyTests
    {
        private ILogger logger;
        private IWellDapperProxy dapperProxy;
        private Mock<IUserNameProvider> userNameProvider;


        [SetUp]
        public void SetUp()
        {
            logger = new NLogger();
            dapperProxy = new WellDapperProxy(new WellDbConfiguration());
            userNameProvider = new Mock<IUserNameProvider>();
            userNameProvider.Setup(x => x.GetUserName()).Returns("TestingTestin123");
        }

        [Test]
        [Explicit]
        public void WhenExceuteExceptionParamtersReset()
        {
            var routeHeaderRepository = new RouteHeaderRepository(logger, dapperProxy, userNameProvider.Object);

            var routeHeader = RouteHeaderFactory.New.With(x => x.RouteOwnerId = 0).Build();
            try
            {
                //This Should Throw
                routeHeaderRepository.Save(routeHeader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);   
            }
            try
            {
                var parameters = new List<GetByNumberDateBranchFilter>
                {
                    new GetByNumberDateBranchFilter { BranchId = 3,  RouteDate =  new DateTime(2017, 09, 01),  RouteNumber = "117" }
                };
                //this should not throw as parameters are reset to Null
                routeHeaderRepository.GetByNumberDateBranch(parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail("Should not throw an exeptionb");
            }
            
        }

    }
}