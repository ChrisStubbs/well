namespace PH.Well.UnitTests.Domain.Extensions
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Well.Domain.Enums;
    using Well.Domain.Extensions;

    [TestFixture]
    public class JobExstensionTests
    {
        [Test]
        public void ShouldReturnTheCorrectWellStatus()
        {
            foreach (var jobStatus in Enum.GetValues(typeof(JobStatus)).Cast<JobStatus>())
            {
                switch (jobStatus)
                {
                    case JobStatus.NotDefined:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Unknown));
                        break;
                    case JobStatus.AwaitingInvoice:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Planned));
                        break;
                    case JobStatus.InComplete:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Invoiced));
                        break;
                    case JobStatus.Clean:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.Exception:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.Resolved:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.DocumentDelivery:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.CompletedOnPaper:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Complete));
                        break;
                    case JobStatus.Bypassed:
                        Assert.That(jobStatus.ToWellStatus(), Is.EqualTo(WellStatus.Bypassed));
                        break;
                    default:
                        Assert.IsTrue(false, "Add the new status ToWellStatus Method ");
                        break;
                }
            }
        }
    }
}