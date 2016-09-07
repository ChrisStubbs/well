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
                var deleteType = WellDeleteType.SoftDelete;
                var isSoftDelete = deleteType == WellDeleteType.SoftDelete;

                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteById))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailArttributesDeleteByJobDetailId))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteDamageReasonsByJobDetailId))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());


                this.repository.DeleteJobDetailById(id, deleteType);


                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.JobDetailArttributesDeleteByJobDetailId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.JobDetailDeleteDamageReasonsByJobDetailId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());
            }
        }

        public class TheSaveJobDetailMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldSaveJobDetail()
            {
                var jobDetail = JobDetailFactory.New.Build();

                string sprocName = "JobDetail_Insert";
                dapperProxy.Setup(x => x.WithStoredProcedure(sprocName)).Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Barcode", jobDetail.BarCode, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(
                        x => x.AddParameter("OriginalDespatchQty", jobDetail.OriginalDispatchQty, DbType.Decimal, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SkuWeight", jobDetail.SkuWeight, DbType.Decimal, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SkuCube", jobDetail.SkuCube, DbType.Decimal, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField1", jobDetail.TextField1, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField2", jobDetail.TextField2, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField3", jobDetail.TextField3, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField4", jobDetail.TextField4, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField5", jobDetail.TextField5, DbType.String, null))
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
                dapperProxy.Verify(x => x.AddParameter("Barcode", jobDetail.BarCode, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(
                    x => x.AddParameter("OriginalDespatchQty", jobDetail.OriginalDispatchQty, DbType.Decimal, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SkuWeight", jobDetail.SkuWeight, DbType.Decimal, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SkuCube", jobDetail.SkuCube, DbType.Decimal, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField1", jobDetail.TextField1, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField2", jobDetail.TextField2, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField3", jobDetail.TextField3, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField4", jobDetail.TextField4, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField5", jobDetail.TextField5, DbType.String, null),
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
            }
        }

        public class TheSaveJobAttributeMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldSaveJobAttribute()
            {
                var jobDetail = JobDetailFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailAttributeCreateOrUpdate))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", jobDetail.EntityAttributes[0].Id, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Code", jobDetail.EntityAttributes[0].Code, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(
                        x => x.AddParameter("Value", jobDetail.EntityAttributes[0].Value1, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", jobDetail.Id, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] {1});

                this.repository.AddJobDetailAttributes(jobDetail.EntityAttributes[0]);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailAttributeCreateOrUpdate),
                    Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("Id", jobDetail.EntityAttributes[0].Id, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(
                    x => x.AddParameter("Code", jobDetail.EntityAttributes[0].Code, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(
                    x => x.AddParameter("Value", jobDetail.EntityAttributes[0].Value1, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", jobDetail.Id, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }
        }
    }
}
