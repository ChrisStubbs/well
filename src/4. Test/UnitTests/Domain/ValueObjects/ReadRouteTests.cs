namespace PH.Well.UnitTests.Domain.ValueObjects
{
    using NUnit.Framework;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class ReadRouteTests
    {

        [Test]
        public void ShouldReturnFullNameIfOneAsignees()
        {
            var r = new Route();
            r.Assignees.Add(new RouteAssignees {Name="Mick Chidders"});
            Assert.That(r.Assignee, Is.EqualTo("Mick Chidders"));
        }

        [Test]
        public void ShouldReturnUnallocatedIfNoAsignees()
        {
            var r = new Route();
            Assert.That(r.Assignee, Is.EqualTo("Unallocated"));
        }

        [Test]
        public void ShouldReturnCommaSeperatedListOfInitialsIfMoreThanOneAssignee()
        {
            var r = new Route();
            r.Assignees.Add(new RouteAssignees { Name = "Mick Chidders" });
            r.Assignees.Add(new RouteAssignees { Name = "Lee Grunion" });
            r.Assignees.Add(new RouteAssignees { Name = "Chubbs" });
            r.Assignees.Add(new RouteAssignees { Name = "Enrri Portugals Finest" });

            Assert.That(r.Assignee, Is.EqualTo("MC, LG, C, EPF"));
        }
    }
}
