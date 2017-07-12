namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Common.Extensions;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class AmendmentRepositoryTests
    {
        private Mock<ILogger> logger;
        private Mock<IWellDapperProxy> dapperProxy;
        private AmendmentRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.repository = new AmendmentRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetAmendmentsMethod : AmendmentRepositoryTests
        {
            [Test]
            public void ShouldReturnAmendments()
            {
                var jobIdsList = new List<int> {1};

                var amends = new List<Amendment>
                {
                    new Amendment
                    {
                        JobId = 1,
                        AccountNumber = "1000.123",
                        AmenderName = "Amanda Mender",
                        BranchId = 2,
                        InvoiceNumber = "1232466",
                        AmendmentLines = new List<AmendmentLine>
                        {
                            new AmendmentLine
                            {
                                JobId = 1,
                                ProductCode = "989898",
                                DeliveredQuantity = 12,
                                AmendedDeliveredQuantity = 10,
                                ShortTotal = 0,
                                AmendedShortTotal = 1,
                                DamageTotal = 0,
                                AmendedDamageTotal = 1,
                                RejectedTotal = 0,
                                AmendedRejectedTotal = 0
                            }
                        }
                    }
                };


                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.GetAmendments))
                  .Returns(this.dapperProxy.Object);
                this.dapperProxy.Setup(x => x.AddParameter("jobIds",It.IsAny<DataTable>(), DbType.Object , null))
                  .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, List<Amendment>>>()))
                    .Returns(amends);

                repository.GetAmendments(jobIdsList);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.GetAmendments), Times.Once);
                this.dapperProxy.Verify(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, List<Amendment>>>()), Times.Once);
                this.dapperProxy.Verify(x => x.AddParameter("jobIds", It.Is<DataTable>(
                    dt=> (int)dt.Rows[0][0] == 1 && dt.Rows.Count == 1)
                    , DbType.Object, null),
                    Times.Once);
            }
        }
    }
}
