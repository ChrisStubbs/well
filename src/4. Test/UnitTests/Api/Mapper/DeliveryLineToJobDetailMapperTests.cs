namespace PH.Well.UnitTests.Api.Mapper
{
    using NUnit.Framework;

    using PH.Well.Api.Mapper;
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    [TestFixture]
    public class DeliveryLineToJobDetailMapperTests
    {
        private DeliveryLineToJobDetailMapper mapper;

        [SetUp]
        public void Setup()
        {
            this.mapper = new DeliveryLineToJobDetailMapper();
        }

        [Test]
        public void ShouldMapDeliveryLineToJobDetail()
        {
            var model = new DeliveryLineModel
            {
                ShortQuantity = 99,
                JobDetailReasonId = 5,
                JobDetailSourceId = 2,
                ShortsActionId = 3
            };
            
            var damage1 = new DamageModel { JobDetailReasonId = 2, JobDetailSourceId = 3, Quantity = 51 };
            var damage2 = new DamageModel { JobDetailReasonId = 1, JobDetailSourceId = 2, Quantity = 21 };

            model.Damages.Add(damage1);
            model.Damages.Add(damage2);

            var jobDetail = new JobDetail() { Id = 34 };

            this.mapper.Map(model, jobDetail);

            Assert.That(jobDetail.ShortQty, Is.EqualTo(model.ShortQuantity));
            Assert.That(jobDetail.JobDetailReasonId, Is.EqualTo(model.JobDetailReasonId));
            Assert.That(jobDetail.JobDetailSourceId, Is.EqualTo(model.JobDetailSourceId));
            Assert.That(jobDetail.ShortsActionId, Is.EqualTo(model.ShortsActionId));

            Assert.That(jobDetail.JobDetailDamages.Count, Is.EqualTo(2));

            Assert.That((int)jobDetail.JobDetailDamages[0].JobDetailReason, Is.EqualTo(model.Damages[0].JobDetailReasonId));
            Assert.That((int)jobDetail.JobDetailDamages[0].JobDetailSource, Is.EqualTo(model.Damages[0].JobDetailSourceId));
            Assert.That(jobDetail.JobDetailDamages[0].JobDetailId, Is.EqualTo(jobDetail.Id));
            Assert.That(jobDetail.JobDetailDamages[0].Qty, Is.EqualTo(model.Damages[0].Quantity));

            Assert.That((int)jobDetail.JobDetailDamages[1].JobDetailReason, Is.EqualTo(model.Damages[1].JobDetailReasonId));
            Assert.That((int)jobDetail.JobDetailDamages[1].JobDetailSource, Is.EqualTo(model.Damages[1].JobDetailSourceId));
            Assert.That(jobDetail.JobDetailDamages[1].JobDetailId, Is.EqualTo(jobDetail.Id));
            Assert.That(jobDetail.JobDetailDamages[1].Qty, Is.EqualTo(model.Damages[1].Quantity));
        }
    }
}