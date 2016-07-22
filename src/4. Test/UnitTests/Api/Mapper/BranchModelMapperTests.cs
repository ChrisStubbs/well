namespace PH.Well.UnitTests.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using PH.Well.Api.Mapper;
    using PH.Well.Domain;

    [TestFixture]
    public class BranchModelMapperTests
    {
        [Test]
        public void ShouldMapCorrectly()
        {
            var branches = new List<Branch>();
            var usersBranches = new List<Branch>();

            branches.Add(new Branch { Id = 1, Name = "Medway" });
            branches.Add(new Branch { Id = 2, Name = "Birtley" });

            usersBranches.Add(new Branch { Id = 1 });

            var mappedModels = new BranchModelMapper().Map(branches, usersBranches);

            Assert.That(mappedModels.Count(), Is.EqualTo(2));

            var selectedBranch = mappedModels.Where(x => x.Selected).ToList();

            Assert.That(selectedBranch[0].Selected, Is.True);
        }
    }
}