using Moq;
using NUnit.Framework;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class DeliveryStatusServiceTests
    {
        private Mock<IJobRepository> jobRepository;
        private Mock<IJobDetailRepository> jobDetailRepository;
        
        private DeliveryStatusService service;

        [SetUp]
        public void Setup()
        {
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.service = new DeliveryStatusService(this.jobRepository.Object, this.jobDetailRepository.Object);
        }

        public class TheSetStatusMethod : DeliveryStatusServiceTests
        {
            public void ShouldSetTheJobToAnException()
            {
                var job = JobFactory.New.Build();
                // TODO this.service.SetStatus(job);
            }
            
        }
    }
}
