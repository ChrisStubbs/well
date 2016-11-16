namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class JobDetailRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private JobDetailRepository repository;
        private string UserName = "TestUser";

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new JobDetailRepository(this.logger.Object, this.dapperProxy.Object);
            this.repository.CurrentUser = UserName;
        }

        public class TheGetByIdMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailGet))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", null, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LineNumber", null, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()));

                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailGet), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(
                    x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()),
                    Times.Once());

            }
        }


        public class TheGetByJobIdMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailGet))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", null, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LineNumber", null, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()));

                var result = repository.GetByJobId(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailGet), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobId", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()),Times.Once);
            }
        }

        public class TheDeleteJobDetailByIdMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteById))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());
                
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteDamageReasonsByJobDetailId))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                this.repository.DeleteJobDetailById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteDamageReasonsByJobDetailId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());
            }
        }

        public class TheSaveJobDetailMethod : JobDetailRepositoryTests
        {
            // TODO fix test
            /*[Test]
            public void ShouldSaveJobDetail()
            {
                var jobDetail = JobDetailFactory.New.Build();

                string sprocName = "JobDetail_Insert";
                dapperProxy.Setup(x => x.WithStoredProcedure(sprocName)).Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null))
                    .Returns(dapperProxy.Object);   
                dapperProxy.Setup(x => x.AddParameter("PHProductCode", jobDetail.PhProductCode, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(
                        x => x.AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Decimal, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PHProductType", jobDetail.PhProductType, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PackSize", jobDetail.PackSize, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SingleOrOuter", jobDetail.SingleOrOuter, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SSCCBarcode", jobDetail.SsccBarcode, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SubOuterDamageTotal", jobDetail.SubOuterDamageTotal, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", jobDetail.JobId, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(
                        x => x.AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("CreatedBy", jobDetail.CreatedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DateCreated", jobDetail.DateCreated, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DbType>(), null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] {1});

                this.repository.Save(jobDetail);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(sprocName), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PHProductCode", jobDetail.PhProductCode, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(
                    x => x.AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PHProductType", jobDetail.PhProductType, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PackSize", jobDetail.PackSize, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SingleOrOuter", jobDetail.SingleOrOuter, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SSCCBarcode", jobDetail.SsccBarcode, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SubOuterDamageTotal", jobDetail.SubOuterDamageTotal, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("JobId", jobDetail.JobId, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(
                    x => x.AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("CreatedBy", jobDetail.CreatedBy, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DateCreated", jobDetail.DateCreated, DbType.DateTime, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime, null),
                    Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));
            }*/
        }
    }
}
