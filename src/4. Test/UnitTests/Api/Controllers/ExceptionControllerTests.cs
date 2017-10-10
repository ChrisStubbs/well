namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Api.Controllers;
    using Well.Api.Mapper.Contracts;
    using Well.Common.Contracts;
    using Well.Services.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class ExceptionControllerTests : BaseControllerTests<ExceptionController>
    {
        private Mock<ILineItemActionService> lineItemActionService;
        private Mock<ILineItemSearchReadRepository> lineItemSearchReadRepository;
        private Mock<ILineItemExceptionMapper> lineItemExceptionMapper;
        private Mock<IJobRepository> jobRepository;
        private Mock<IJobService> jobService;
        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public virtual void Setup()
        {
            lineItemActionService = new Mock<ILineItemActionService>(MockBehavior.Strict);
            lineItemSearchReadRepository = new Mock<ILineItemSearchReadRepository>(MockBehavior.Strict);
            lineItemExceptionMapper = new Mock<ILineItemExceptionMapper>(MockBehavior.Strict);
            jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            jobService = new Mock<IJobService>(MockBehavior.Strict);

            Controller = new ExceptionController(
                lineItemActionService.Object,
                lineItemSearchReadRepository.Object,
                lineItemExceptionMapper.Object,
                jobRepository.Object,
                jobService.Object,
                userNameProvider.Object
            );
            SetupController();
        }


        public class ThePatchMethod : ExceptionControllerTests
        {
            [Test]
            public void ShouldThrowExceptionIfNoJob()
            {
                var update = new EditLineItemException { JobId = 2 };
                jobRepository.Setup(x => x.GetById(2)).Returns((Job)null);

                Assert.That(() => Controller.Patch(update), Throws.Exception);
            }

            [Test]
            public void ShouldThrowExceptionIfJobNotEditable()
            {
                var update = new EditLineItemException { JobId = 2 };
                var j = new Job {ResolutionStatus = ResolutionStatus.Imported};
                jobRepository.Setup(x => x.GetById(2)).Returns(j);
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                jobService.Setup(x => x.CanEdit(j, "Me")).Returns("no");
        
                Assert.That(() => Controller.Patch(update), Throws.Exception);
            }

            [Test]
            public void ShouldCallSaveAndMap()
            {
                var j = new Job { ResolutionStatus = ResolutionStatus.ActionRequired };
                var update = new EditLineItemException { Id = 4, JobId = 2 };
                var li = new LineItem();
                var retVal = new EditLineItemException();

                jobRepository.Setup(x => x.GetById(2)).Returns(j);
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                jobService.Setup(x => x.CanEdit(j, "Me")).Returns(string.Empty);


                lineItemActionService.Setup(x => x.SaveLineItemActions(j, update.Id, update.LineItemActions)).Returns(li);
                lineItemExceptionMapper.Setup(x => x.Map(li)).Returns(retVal);

                var result = Controller.Patch(update);

                Assert.That(result, Is.EqualTo(retVal));
                lineItemActionService.Verify(x => x.SaveLineItemActions(It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<IEnumerable<LineItemAction>>()), Times.Once);
                lineItemExceptionMapper.Verify(x => x.Map(It.IsAny<LineItem>()), Times.Once);

            }
        }
    }
}
