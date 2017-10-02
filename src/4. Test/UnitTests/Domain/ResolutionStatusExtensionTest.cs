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

                    if (resolutionStatus == ResolutionStatus.DriverCompleted
                        || resolutionStatus == ResolutionStatus.ManuallyCompleted
                        || resolutionStatus == ResolutionStatus.ActionRequired
                        || resolutionStatus == ResolutionStatus.PendingSubmission
                        || resolutionStatus == ResolutionStatus.PendingApproval
                        || resolutionStatus == ResolutionStatus.ApprovalRejected
                        )
                    {
                        Assert.True(resolutionStatus.IsEditable());
                    }
                    else if (resolutionStatus == ResolutionStatus.Imported
                        || resolutionStatus == ResolutionStatus.Approved
                        || resolutionStatus == ResolutionStatus.Resolved
                        || resolutionStatus == ResolutionStatus.Credited
                        || resolutionStatus == ResolutionStatus.Closed
                        || resolutionStatus == ResolutionStatus.Invalid)
                    {
                        Assert.False(resolutionStatus.IsEditable());
                    }
                    else
                    {
                        Assert.True(false, "New Resolution Status Added: Consider whether you need to be able to edit exceptions");
                    }
                }
            }
        }
    }
}
