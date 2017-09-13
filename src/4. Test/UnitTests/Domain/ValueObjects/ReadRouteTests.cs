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
            r.Assignees.Add(new Assignee {Name="Mick Chidders"});
            Assert.That(r.Assignee, Is.EqualTo("Mick Chidders"));
        }

        [Test] public void ShouldReturnNullIfNoAsignees()
        {
            var r = new Route();
            Assert.That(r.Assignee, Is.Null);
        }

        [Test]
        public void ShouldReturnCommaSeperatedListOfInitialsIfMoreThanOneAssignee()
        {
            var r = new Route();
            r.Assignees.Add(new Assignee { Name = "Mick Chidders" });
            r.Assignees.Add(new Assignee { Name = "Lee Grunion" });
            r.Assignees.Add(new Assignee { Name = "Chubbs" });
            r.Assignees.Add(new Assignee { Name = "Enrri Portugals Finest" });

            Assert.That(r.Assignee, Is.EqualTo("MC, LG, C, EPF"));
        }
    }
}
