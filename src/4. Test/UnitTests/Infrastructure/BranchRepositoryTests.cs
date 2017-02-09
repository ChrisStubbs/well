namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class BranchRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private BranchRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);
            
            this.repository = new BranchRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetAllMethod : BranchRepositoryTests
        {
            [Test]
            public void ShouldReturnAllBranches()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.BranchesGet)).Returns(this.dapperProxy.Object);
                this.dapperProxy.Setup(x => x.Query<Branch>()).Returns(new List<Branch>());

                this.repository.GetAll();

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.BranchesGet), Times.Once);
                this.dapperProxy.Verify(x => x.Query<Branch>(), Times.Once);
            }
        }

        public class TheGetAllValidBranchesMethod : BranchRepositoryTests
        {
            [Test]
            public void ShouldOnlyReturnRealBranches()
            {
                var validBranch1 = BranchFactory.New.With(x => x.Id = (int)Well.Domain.Enums.Branch.Belfast).Build();
                var validBranch2 = BranchFactory.New.With(x => x.Id = (int)Well.Domain.Enums.Branch.Hemel).Build();
                var invalidBranch = BranchFactory.New.With(x => x.Id = (int)Well.Domain.Enums.Branch.NotDefined).Build();

                var branches = new List<Branch> { validBranch1, validBranch2, invalidBranch };

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.BranchesGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<Branch>()).Returns(branches);

                var result = this.repository.GetAllValidBranches().ToList();

                Assert.That(result.Count(), Is.EqualTo(2));

                var result1 = result[0];
                var result2 = result[1];

                Assert.That(result1.Id, Is.Not.EqualTo((int)Well.Domain.Enums.Branch.NotDefined));
                Assert.That(result2.Id, Is.Not.EqualTo((int)Well.Domain.Enums.Branch.NotDefined));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.BranchesGet), Times.Once);

                this.dapperProxy.Verify(x => x.Query<Branch>(), Times.Once);
            }
        }

        public class TheDeleteUserBranchesMethod : BranchRepositoryTests
        {
            [Test]
            public void ShouldDeleteAllTheUsersBranches()
            {
                var user = UserFactory.New.Build();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.DeleteUserBranches))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UserId", user.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.DeleteUserBranches(user);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.DeleteUserBranches), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UserId", user.Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

        public class TheSaveBranchesForUserMethod : BranchRepositoryTests
        {
            [Test]
            public void ShouldSaveTheUserBranchPreferences()
            {
                var branches = new List<Branch> { BranchFactory.New.Build(), BranchFactory.New.Build() };
                var user = UserFactory.New.Build();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.SaveUserBranch))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UserId", user.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("BranchId", branches[0].Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreatedBy", It.IsAny<string>(), DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", It.IsAny<string>(), DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.SaveBranchesForUser(branches, user);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.SaveUserBranch), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.AddParameter("UserId", user.Id, DbType.Int32, null), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.AddParameter("BranchId", branches[0].Id, DbType.Int32, null), Times.Exactly(2));

                this.dapperProxy.Verify(x => x.Execute(), Times.Exactly(2));
            }
        }

        public class TheGetBranchesForUserMethod : BranchRepositoryTests
        {
            [Test]
            public void ShouldreturnAllTheBranchIdsForAGivenUsername()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.GetBranchesForUser))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Name", "foo", DbType.String, 255))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<Branch>()).Returns(new List<Branch> { new Branch { Id = 1 }, new Branch { Id = 2 }, new Branch { Id = 3 }});

                var branchIds = this.repository.GetBranchesForUser("foo");

                Assert.That(branchIds.Count(), Is.EqualTo(3));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.GetBranchesForUser), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Name", "foo", DbType.String, 255), Times.Once);

                this.dapperProxy.Verify(x => x.Query<Branch>(), Times.Once);
            }
        }

        public class TheGetBRanchIdForJobMethod : BranchRepositoryTests
        {
            [Test]
            public void ShouldReturnTheBranchIdForAJob()
            {
                var jobId = 43;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.GetBranchIdForJob))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("jobId", jobId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new List<int> { 1 });

                this.repository.GetBranchIdForJob(jobId);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.GetBranchIdForJob), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("jobId", jobId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);
            }
        }
    }
}