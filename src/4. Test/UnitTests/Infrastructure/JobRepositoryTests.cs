

using System;
using Dapper;

namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using Factories;
    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;
    
    using Repositories;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;


    [TestFixture]
    public class JobRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;
        private Mock<IUserNameProvider> userNameProvider;

        private JobRepository repository;
        private string UserName = "TestUser";

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("user");
            this.repository = new JobRepository(this.logger.Object, this.dapperProxy.Object, userNameProvider.Object);
            //////this.repository.CurrentUser = UserName;
        }

        public class TheGetCreditActionReasonsByIdMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldReturnCreditActionReasons()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PDACreditReasonId", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<PodActionReasons>()).Returns(new List<PodActionReasons>());

                var result = repository.GetPodActionReasonsById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("PDACreditReasonId", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<PodActionReasons>(), Times.Once());
            }
        }

        public class TheDeleteJobByIdMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDeleteById))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                this.repository.DeleteJobById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDeleteById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());
            }
        }

        public class TheSaveGrnMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldSaveTheGrnAgainstTheJob()
            {
                var jobId = 101;
                var grn = "3323332111";

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.SaveGrn))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("JobId", jobId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Grn", grn, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.SaveGrn(jobId, grn);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.SaveGrn), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("JobId", jobId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Grn", grn, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }
    }
}
