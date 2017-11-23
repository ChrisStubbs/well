namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
    using Factories;
    using Moq;
    using NUnit.Framework;

    using PH.Well.UnitTests.Api.Controllers;

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

        private Mock<IUserNameProvider> userNameProvider;

        private Mock<IDbConfiguration> dbConfig;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.dbConfig = new Mock<IDbConfiguration>();

            this.userNameProvider.Setup(x => x.GetUserName()).Returns("TestUser");
            dapperProxy.Setup(x => x.DbConfiguration).Returns(dbConfig.Object);
            this.repository = new JobDetailRepository(this.logger.Object, this.dapperProxy.Object, this.userNameProvider.Object);
            //////this.repository.CurrentUser = UserName;
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
                dapperProxy.Setup(x => x.QueryMultiples(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()))
                    .Returns(new List<JobDetail>());

                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailGet), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(
                    x => x.QueryMultiples(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()),
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
                dapperProxy.Setup(x => x.QueryMultiples(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()))
                    .Returns(new List<JobDetail>());

                var result = repository.GetByJobId(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailGet), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobId", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.QueryMultiples(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<JobDetail>>>()),Times.Once);
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
            [Test]
            public void ShouldSaveJobDetail()
            {
                var jobDetail = JobDetailFactory.New.Build();

                string sprocName = "JobDetail_Insert";
                dapperProxy.Setup(x => x.WithStoredProcedure(sprocName)).Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null))
                    .Returns(dapperProxy.Object);   
                dapperProxy.Setup(x => x.AddParameter("PHProductCode", jobDetail.PhProductCode, DbType.String, null))
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
                dapperProxy.Setup(x => x.AddParameter("SSCCBarcode", jobDetail.SSCCBarcode, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SubOuterDamageTotal", jobDetail.SubOuterDamageTotal, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", jobDetail.JobId, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(
                        x => x.AddParameter("ShortsStatus", jobDetail.ShortsStatus, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("CreatedBy", jobDetail.CreatedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DateCreated", jobDetail.DateCreated, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsHighValue", jobDetail.IsHighValue, DbType.Boolean, null))
                 .Returns(dapperProxy.Object);


                dapperProxy.Setup(x => x.AddParameter(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DbType>(), null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] {1});

                this.repository.Save(jobDetail);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(sprocName), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PHProductCode", jobDetail.PhProductCode, DbType.String, null),
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
                dapperProxy.Verify(x => x.AddParameter("SSCCBarcode", jobDetail.SSCCBarcode, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SubOuterDamageTotal", jobDetail.SubOuterDamageTotal, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("JobId", jobDetail.JobId, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(
                    x => x.AddParameter("ShortsStatus", jobDetail.ShortsStatus, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("CreatedBy", jobDetail.CreatedBy, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DateCreated", jobDetail.DateCreated, DbType.DateTime, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("UpdatedBy", jobDetail.UpdatedBy, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DateUpdated", jobDetail.DateUpdated, DbType.DateTime, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("IsHighValue", jobDetail.IsHighValue, DbType.Boolean, null),
                  Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));
            }
        }

        public class TheUpdateMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldUpdateTheJobDetail()
            {
                var jobDetail = JobDetailFactory.New.Build();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailUpdate)).Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Id", jobDetail.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("DeliveredQty", jobDetail.DeliveredQty, DbType.Int32, null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32, null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("JobDetailReasonId", jobDetail.JobDetailReasonId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("JobDetailSourceId", jobDetail.JobDetailSourceId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("ShortsStatus", jobDetail.ShortsStatus, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("ShortsActionId", jobDetail.ShortsActionId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("LineDeliveryStatus", jobDetail.LineDeliveryStatus ?? "Unknown", DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("SubOuterDamageQty", jobDetail.SubOuterDamageTotal, DbType.Int16, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("ProductCode", jobDetail.PhProductCode, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("ProductDescription", jobDetail.ProdDesc, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("ProductType", jobDetail.PhProductType, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("PackSize", jobDetail.PackSize, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("SingleOrOuter", jobDetail.SingleOrOuter, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("Barcode", jobDetail.SSCCBarcode, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Decimal, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("UpdatedBy", "TestUser", DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                        x => x.AddParameter("NetPrice", jobDetail.NetPrice, DbType.Decimal, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                      x => x.AddParameter("UpliftActionId", jobDetail.UpliftAction, DbType.Int16, null))
                  .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(
                  x => x.AddParameter("IsSubOuterQuantity", jobDetail.IsSubOuterQuantity, DbType.Boolean, null))
              .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Update(jobDetail);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailUpdate), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Id", jobDetail.Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("DeliveredQty", jobDetail.DeliveredQty, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("JobDetailReasonId", jobDetail.JobDetailReasonId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("JobDetailSourceId", jobDetail.JobDetailSourceId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("ShortsStatus", jobDetail.ShortsStatus, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("ShortsActionId", jobDetail.ShortsActionId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("LineDeliveryStatus", jobDetail.LineDeliveryStatus ?? "Unknown", DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("SubOuterDamageQty", jobDetail.SubOuterDamageTotal, DbType.Int16, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("ProductCode", jobDetail.PhProductCode, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("ProductDescription", jobDetail.ProdDesc, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("ProductType", jobDetail.PhProductType, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("PackSize", jobDetail.PackSize, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("SingleOrOuter", jobDetail.SingleOrOuter, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("Barcode", jobDetail.SSCCBarcode, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Decimal, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("UpdatedBy", "TestUser", DbType.String, null), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);

                this.dapperProxy.Verify(
                        x => x.AddParameter("OriginalDespatchQty", jobDetail.OriginalDespatchQty, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(
                    x => x.AddParameter("NetPrice", jobDetail.NetPrice, DbType.Decimal, null), Times.Once);

                this.dapperProxy.Verify(
                x => x.AddParameter("UpliftActionId", jobDetail.UpliftAction, DbType.Int16, null), Times.Once);

                this.dapperProxy.Verify(
                x => x.AddParameter("IsSubOuterQuantity", jobDetail.IsSubOuterQuantity, DbType.Boolean, null), Times.Once);
            }
        }
    }
}
