namespace PH.Well.UnitTests.Api.Mapper
{
    using NUnit.Framework;

    using PH.Well.Api.Mapper;
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    [TestFixture]
    public class CleanPreferenceMapperTests
    {
        private CleanPreferenceMapper mapper;

        [SetUp]
        public void Setup()
        {
            this.mapper = new CleanPreferenceMapper();
        }

        public class TheMapMethodModelToEntity : CleanPreferenceMapperTests
        {
            [Test]
            public void ShouldMapCorrectly()
            {
                var model = new CleanPreferenceModel { Id = 101, Days = 1 };

                model.Branches.Add(new Branch());
                model.Branches.Add(new Branch());

                var cleanPreference = this.mapper.Map(model);

                Assert.That(cleanPreference.Id, Is.EqualTo(model.Id));
                Assert.That(cleanPreference.Days, Is.EqualTo(model.Days));
                Assert.That(cleanPreference.Branches.Count, Is.EqualTo(2));
            }
        }

        public class TheMapMethodEntityToModel : CleanPreferenceMapperTests
        {
            [Test]
            public void ShouldMapCorrectly()
            {
                var cleanPreference = new CleanPreference { Id = 101, Days = 1 };

                cleanPreference.Branches.Add(new Branch { Name = "foo" });
                cleanPreference.Branches.Add(new Branch { Name = "bar" });

                var model = this.mapper.Map(cleanPreference);

                Assert.That(model.Id, Is.EqualTo(cleanPreference.Id));
                Assert.That(model.Days, Is.EqualTo(cleanPreference.Days));
                Assert.That(model.BranchName, Is.EqualTo("foo, bar"));
                Assert.That(model.Branches.Count, Is.EqualTo(2));
            }
        }
    }
}