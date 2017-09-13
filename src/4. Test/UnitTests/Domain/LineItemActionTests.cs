using System.Collections;
using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Domain
{
    [TestFixture]
    class LineItemActionTests
    {
        [Test]
        [TestCaseSource(typeof(LineItemActionTestsSource), nameof(LineItemActionTestsSource.TestCases))]
        public bool Should_HasChanges(LineItemAction testValue)
        {
            var sut = LineItemActionFactory.New.GenerateForHasChange().Build();

            return sut.HasChanges(testValue);
        }
    }

    class LineItemActionTestsSource
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(LineItemActionFactory.New.GenerateForHasChange().Build())
                    .Returns(false);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ExceptionType = Well.Domain.Enums.ExceptionType.Bypass).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Quantity = 19).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Source = Well.Domain.Enums.JobDetailSource.Assembler).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Reason = Well.Domain.Enums.JobDetailReason.AccumulatedDamages).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ReplanDate = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.SubmittedDate = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ApprovalDate = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ApprovedBy = "").Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ActionedBy = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Originator = Well.Domain.Enums.Originator.Customer).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.DeliveryAction = Well.Domain.Enums.DeliveryAction.Close).Build())
                    .Returns(true);
            }
        }
    }
}
