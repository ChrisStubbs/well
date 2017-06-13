using NUnit.Framework;
using PH.Well.Domain;

namespace PH.Well.UnitTests.Domain
{
    [TestFixture]
    class JobDetailActionTests
    {
        [Test]
        public void Should_GetString()
        {
            var sut = new JobDetailAction();
            var result = string.Format("Action: {0}, Quantity: {1}, Status: {2}",
                Well.Domain.Enums.EventAction.Credit,
                33,
                Well.Domain.Enums.ActionStatus.Draft);

            sut.Action = Well.Domain.Enums.EventAction.Credit;
            sut.Quantity = 33;
            sut.Status = Well.Domain.Enums.ActionStatus.Draft;

            Assert.That(sut.GetString(), Is.EqualTo(result));
        }
    }
}
