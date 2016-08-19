namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;

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
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailGetById)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<JobDetail>()).Returns(new List<JobDetail>());

                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailGetById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<JobDetail>(), Times.Once());
            }
        }

        public class TheSaveJobDetailMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldSaveJobDetail()
            {
                var jobDetail = JobDetailFactory.New.Build();


                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailCreateOrUpdate)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter(It.IsAny<string>(), It.IsAny<object>(),It.IsAny<DbType>(), null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsDeleted", jobDetail.IsDeleted, DbType.Boolean, null)).Returns(dapperProxy.Object);
               
                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });
   
                this.repository.CreateOrUpdate(jobDetail);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailCreateOrUpdate), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Barcode", jobDetail.BarCode, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("OriginalDespatchQty", jobDetail.OriginalDispatchQty, DbType.Decimal, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ProdDesc", jobDetail.ProdDesc, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("OrderedQty", jobDetail.OrderedQty, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ShortQty", jobDetail.ShortQty, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SkuWeight", jobDetail.SkuWeight, DbType.Decimal, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SkuCube", jobDetail.SkuCube, DbType.Decimal, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("UnitMeasure", jobDetail.UnitMeasure, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField1", jobDetail.TextField1, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField2", jobDetail.TextField2, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField3", jobDetail.TextField3, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField4", jobDetail.TextField4, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("TextField5", jobDetail.TextField5, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SkuGoodsValue", jobDetail.SkuGoodsValue, DbType.Double, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("JobId", jobDetail.JobId, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("JobDetailStatusId", jobDetail.JobDetailStatusId, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("IsDeleted", jobDetail.IsDeleted, DbType.Boolean, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));
            }
        }

        public class TheSaveJobAttributeMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldSaveJobAttribute()
            {
                var jobDetail = JobDetailFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailAttributeCreateOrUpdate)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", jobDetail.EntityAttributes[0].Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Code", jobDetail.EntityAttributes[0].Code, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Value", jobDetail.EntityAttributes[0].Value1, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", jobDetail.Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.CreateOrUpdateJobDetailAttributes(jobDetail.EntityAttributes[0]);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailAttributeCreateOrUpdate), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("Id", jobDetail.EntityAttributes[0].Id, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Code", jobDetail.EntityAttributes[0].Code, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Value", jobDetail.EntityAttributes[0].Value1, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", jobDetail.Id, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }
        }

        public class TheGetBarcodeLineNumberAndJobId : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldGetByBarcodeLineNumberAndJobId()
            {
                var jobDetail = JobDetailFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailGetByBarcodeLineNumberAndJobId)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Barcode", jobDetail.BarCode, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobId", jobDetail.JobId, DbType.Int32, null)).Returns(this.dapperProxy.Object);

                dapperProxy.Setup(x => x.Query<JobDetail>()).Returns(new List<JobDetail>());

                var result = this.repository.GetByBarcodeLineNumberAndJobId(jobDetail.LineNumber, jobDetail.BarCode, jobDetail.JobId);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailGetByBarcodeLineNumberAndJobId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("LineNumber", jobDetail.LineNumber, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Barcode", jobDetail.BarCode, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("JobId", jobDetail.JobId, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<JobDetail>(), Times.Once());
            }
        }

        public class TheSaveJobDetailDamageMethod : JobDetailRepositoryTests
        {
            [Test]
            public void ShouldSaveJobDetailDamage()
            {
                var jobDetail = JobDetailFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailDamageCreateOrUpdate)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", jobDetail.JobDetailDamages[0].Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("JobDetailId", jobDetail.JobDetailDamages[0].JobDetailId, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Qty", jobDetail.JobDetailDamages[0].Qty, DbType.Decimal, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DamageReasonsId", (int) jobDetail.JobDetailDamages[0].Reason, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.CreateOrUpdateJobDetailDamage(jobDetail.JobDetailDamages[0]);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailDamageCreateOrUpdate), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("Id", jobDetail.JobDetailDamages[0].Id, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("JobDetailId", jobDetail.JobDetailDamages[0].JobDetailId, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Qty", jobDetail.JobDetailDamages[0].Qty, DbType.Decimal, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DamageReasonsId", (int)jobDetail.JobDetailDamages[0].Reason, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }
        }


    }
}
