//namespace PH.Well.UnitTests.Api.Controllers
//{
//    using Moq;
//    using NUnit.Framework;
//    using Repositories.Contracts;
//    using Well.Api.Controllers;
//    using Well.Api.Mapper.Contracts;
//    using Well.Services.Contracts;

//    [TestFixture]
//    public class ExceptionControllerTests : BaseControllerTests<ExceptionController>
//    {
//        private Mock<ILineItemActionService> lineItemActionService;
//        private Mock<ILineItemSearchReadRepository> lineItemSearchReadRepository;
//        private Mock<ILineItemExceptionMapper> lineItemExceptionMapper;
//        private Mock<IJobRepository> jobRepository;

//        [SetUp]
//        public virtual void Setup()
//        {
//            lineItemActionService = new Mock<ILineItemActionService>(MockBehavior.Strict);
//            lineItemSearchReadRepository = new Mock<ILineItemSearchReadRepository>(MockBehavior.Strict);
//            lineItemExceptionMapper = new Mock<ILineItemExceptionMapper>(MockBehavior.Strict);
//            jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
//            Controller = new ExceptionController(
//                lineItemActionService.Object,
//                lineItemSearchReadRepository.Object,
//                lineItemExceptionMapper.Object,
//                jobRepository.Object
//                );
//            SetupController();
//        }
//    }
//}