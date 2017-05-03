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
            var r = new ReadRoute();
            r.Assignees.Add(new ReadRouteAssignees {Name="Mick Chidders"});
            Assert.That(r.Assignee, Is.EqualTo("Mick Chidders"));
        }

        [Test]
        public void ShouldReturnUnallocatedIfNoAsignees()
        {
            var r = new ReadRoute();
            Assert.That(r.Assignee, Is.EqualTo("Unallocated"));
        }

        [Test]
        public void ShouldReturnCommaSeperatedListOfInitialsIfMoreThanOneAssignee()
        {
            var r = new ReadRoute();
            r.Assignees.Add(new ReadRouteAssignees { Name = "Mick Chidders" });
            r.Assignees.Add(new ReadRouteAssignees { Name = "Lee Grunion" });
            r.Assignees.Add(new ReadRouteAssignees { Name = "Chubbs" });
            r.Assignees.Add(new ReadRouteAssignees { Name = "Enrri Portugals Finest" });

            Assert.That(r.Assignee, Is.EqualTo("MC, LG, C, EPF"));
        }
    }
}
