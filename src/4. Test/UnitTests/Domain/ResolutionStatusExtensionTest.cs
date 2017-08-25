namespace PH.Well.UnitTests.Domain
{
    using NUnit.Framework;
    using Well.Domain.Enums;
    using Well.Domain.Extensions;

    [TestFixture]
    public class ResolutionStatusExtensionTest
    {
        public class TheIsEditableMethod : ResolutionStatusExtensionTest
        {
            [Test]
            public void TestEditableStatus()
            {
                foreach (var resolutionStatus in ResolutionStatus.AllStatus)
                {
                    if (resolutionStatus.eValue == ResolutionStatus.DriverCompleted
                        || resolutionStatus.eValue == ResolutionStatus.ActionRequired
                        || resolutionStatus.eValue == ResolutionStatus.PendingSubmission
                        || resolutionStatus.eValue == ResolutionStatus.PendingApproval
                        || resolutionStatus.eValue == ResolutionStatus.ManuallyCompleted
                        )
                    {
                        Assert.True(resolutionStatus.IsEditable());
                    }
                    else
                    {
                        Assert.False(resolutionStatus.IsEditable());
                    }
                }
            }
        }
    }
}
