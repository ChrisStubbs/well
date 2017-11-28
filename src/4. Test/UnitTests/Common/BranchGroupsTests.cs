using System;
using System.Collections.Generic;
using NUnit.Framework;
using PH.Well.Common;

namespace PH.Well.UnitTests.Common
{

    [TestFixture]
    public class BranchGroupsTests
    {
        [Test]
        public void Should_Return_Group_Name()
        {
            var sut = new BranchGroups();

            sut.Groups.Add(new BranchGroups.BranchGroup
            {
                BranchIds = new List<int> { 1,2,3},
                GroupName = "Group1"
            });

            Assert.That(sut.GetGroupNameForBranch(1), Is.EqualTo(sut.Groups[0].GroupName));
        }

        [Test]
        public void Should_Not_Return_Group_Name()
        {
            var sut = new BranchGroups();

            sut.Groups.Add(new BranchGroups.BranchGroup
            {
                BranchIds = new List<int> { 1, 2, 3 },
                GroupName = "Group1"
            });

            Assert.Throws<Exception>(() => sut.GetGroupNameForBranch(8));
        }
    }
}
