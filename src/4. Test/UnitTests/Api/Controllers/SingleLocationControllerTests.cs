using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PH.Well.Api.Controllers;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Domain.Extensions;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;

namespace PH.Well.UnitTests.Api.Controllers
{
    [TestFixture]
    public class SingleLocationControllerTests
    {
        private Mock<ILocationRepository> ILocationRepository;
        private Mock<IAssigneeReadRepository> IAssigneeReadRepository;

        [SetUp]
        public void Setup()
        {
            this.ILocationRepository = new Mock<ILocationRepository>();
            this.IAssigneeReadRepository = new Mock<IAssigneeReadRepository>();

            this.ILocationRepository.Setup(p => p.GetSingleLocationById(It.IsAny<int>())).Returns(LoadSingleLocationMockData());
            this.IAssigneeReadRepository.Setup(p => p.GetByJobIds(It.IsAny<IEnumerable<int>>())).Returns(LoadAssigneeMockData());
        }

        [Test]
        public void Should_Set_JobStatus_Description()
        {
            var sut = new SingleLocationController(this.ILocationRepository.Object, this.IAssigneeReadRepository.Object);

            var result = sut.Get(1);

            Assert.That(result.Details[0].JobStatus, Is.EqualTo(EnumExtensions.GetDescription(WellStatus.Planned)));
        }

        [Test]
        public void Should_Set_JobStatus_Assignee()
        {
            var sut = new SingleLocationController(this.ILocationRepository.Object, this.IAssigneeReadRepository.Object);

            var result = sut.Get(1);

            Assert.That(result.Details.First(p => p.JobId == 1).Assignee.Name, Is.EqualTo("Unallocated"));
            Assert.That(result.Details.First(p => p.JobId == 2).Assignee.Name, Is.EqualTo("Name"));
        }

        private SingleLocation LoadSingleLocationMockData()
        {
            return new SingleLocation
            {
                Details = new List<SingleLocationItems>
                {
                    new SingleLocationItems { JobId = 1, WellStatus = (int)WellStatus.Planned },
                    new SingleLocationItems { JobId = 2, WellStatus = (int)WellStatus.Planned }
                }
            };
        }

        private IEnumerable<Assignee> LoadAssigneeMockData()
        {
            yield return new Assignee { JobId = 2, IdentityName = "IdentityName", Name = "Name" };
        }
    }
}
