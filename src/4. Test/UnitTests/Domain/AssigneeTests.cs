namespace PH.Well.UnitTests.Domain
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class AssigneeTests
    {
        public class TheGetDisplayNamesMethod : AssigneeTests
        {
            [Test]
            public void ShouldReturnNullIfNoAssignees()
            {
                var assignees = new List<Assignee>();
                Assert.IsNull(Assignee.GetDisplayNames(assignees));
            }

            [Test]
            public void ShouldReturnNameIfSingleAssignee()
            {
                var assignees = new List<Assignee> {new Assignee {Name = "KryZ DuZZZick" },
                                                    new Assignee { Name = "KryZ DuZZZick" } };
                Assert.That(Assignee.GetDisplayNames(assignees),Is.EqualTo("KryZ DuZZZick"));
            }

            [Test]
            public void ShouldReturnInitialsMultipleAssignee()
            {
                var assignees = new List<Assignee> {new Assignee { Name = "KryZ DuZZZick" },
                                                    new Assignee { Name = "KryZ DuZZZick" },
                                                    new Assignee { Name = "Chris A Stubba" } };
                Assert.That(Assignee.GetDisplayNames(assignees), Is.EqualTo("KD, CAS"));
            }
        }
    }
}