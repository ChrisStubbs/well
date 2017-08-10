using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.UnitTests.Factories;
using static PH.Well.Domain.Mappers.AutoMapperConfig;

namespace PH.Well.UnitTests.DTOMappers
{
    [TestFixture]
    class JobDetailsDTOTests
    {
        [Test]
        public void JobDetailsDTO_Should_Store_Correct_Values()
        {
            var jobDetailsDTO = JobDetailFactoryDTO.New
            .AddEntityAttribute("NETPRICE", "10.3")
            .AddEntityAttribute("SUBOUTQTY", "11")
            .AddEntityAttributeValue("LINESTATUS", "Exception")
            .AddEntityAttribute("HIGHVALUE", "N")
            .With(p => p.JobDetailDamages.Add(new JobDetailDamageDTO { Qty = 30 }))
            .With(p => p.JobDetailDamages.Add(new JobDetailDamageDTO { Qty = 20 }))
            .With(p => p.ShortQty = 25)
            .With(p => p.SkuGoodsValue = 1.5D)
            .With(p => p.DeliveredQty =77)
            .Build();

            Assert.That(jobDetailsDTO.NetPrice, Is.EqualTo(10.3M));
            Assert.That(jobDetailsDTO.SubOuterDamageTotal, Is.EqualTo(11));
            Assert.That(jobDetailsDTO.LineDeliveryStatus, Is.EqualTo("Exception"));
            Assert.That(jobDetailsDTO.IsChecked, Is.True);
            Assert.That(jobDetailsDTO.CreditValueForThreshold, Is.EqualTo(112.5M));
            Assert.That(jobDetailsDTO.IsHighValue, Is.False);
        }

        [Test]
        public void Should_Map_From_JobDetailsDTO_To_JobDetails()
        {
            var jobDetailsDTO = JobDetailFactoryDTO.New
                .AddEntityAttribute("NETPRICE", "10.3")
                .AddEntityAttribute("SUBOUTQTY", "11")
                .AddEntityAttributeValue("LINESTATUS", "Exception")
                .AddEntityAttribute("HIGHVALUE", "N")
                .Build();

            var sut = Mapper.Map<JobDetailDTO, JobDetail>(jobDetailsDTO);

            Assert.That(sut.NetPrice, Is.EqualTo(jobDetailsDTO.NetPrice));
            Assert.That(sut.SubOuterDamageTotal, Is.EqualTo(jobDetailsDTO.SubOuterDamageTotal));
            Assert.That(sut.LineDeliveryStatus, Is.EqualTo(jobDetailsDTO.LineDeliveryStatus));
            Assert.That(sut.IsHighValue, Is.EqualTo(jobDetailsDTO.IsHighValue));
        }

        [Test]
        public void JobDetailMapper()
        {
            var from = new JobDetailDTO();

            from.ShortQty = 3;
            from.DeliveredQty = 5;

            var to = new JobDetail();

            to = Mapper.Map<JobDetailDTO, JobDetail>(from);
            Assert.That(to.ShortQty, Is.EqualTo(from.ShortQty));
            Assert.That(to.DeliveredQty, Is.EqualTo(from.DeliveredQty));
        }

        [Test]
        public void Should_InvertNegativeFigures()
        {
            var from = new JobDetailDTO
            {
                OriginalDespatchQty = -20,
                DeliveredQty = -15
            };

            var to = new JobDetail();

            to = Mapper.Map<JobDetailDTO, JobDetail>(from);

            Assert.That(() => from.OriginalDespatchQty == (to.OriginalDespatchQty * -1));
            Assert.That(() => from.DeliveredQty == (to.DeliveredQty * -1));
        }
    }
}
