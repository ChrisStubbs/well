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
                    if (resolutionStatus.Value == ResolutionStatus.DriverCompleted
                        || resolutionStatus.Value == ResolutionStatus.ActionRequired
                        || resolutionStatus.Value == ResolutionStatus.PendingSubmission
                        || resolutionStatus.Value == ResolutionStatus.PendingApproval
                        || resolutionStatus.Value == ResolutionStatus.ManuallyCompleted
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
