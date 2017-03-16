namespace PH.Well.UnitTests.Api.Mapper
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using PH.Well.Api.Mapper;
    using PH.Well.Common.Extensions;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    [TestFixture]
    public class DeliveryLinesToModelMapperTests
    {
        private DeliveryLinesToModelMapper mapper;

        [SetUp]
        public void Setup()
        {
            this.mapper = new DeliveryLinesToModelMapper();
        }

        [Test]
        public void TheMapMethod()
        {
            var line1 = new DeliveryLine
            {
                ShortQuantity = 43,
                JobDetailReasonId = 1,
                JobDetailSourceId = 2,
                ShortsActionId = 3
            };

            var damage1 = new Damage
            {
                Quantity = 67,
                JobDetailReasonId = 3,
                JobDetailSourceId = 4,
                DamageActionId = 5
            };

            line1.Damages.Add(damage1);

            var line2 = new DeliveryLine
            {
                ShortQuantity = 2,
                JobDetailReasonId = 2,
                JobDetailSourceId = 3,
                ShortsActionId = 4
            };

            var damage2 = new Damage
            {
                Quantity = 33,
                JobDetailReasonId = 5,
                JobDetailSourceId = 3,
                DamageActionId = 2
            };

            line2.Damages.Add(damage2);

            var lines = new List<DeliveryLine> { line1, line2 };

            var model = this.mapper.Map(lines);

            Assert.That(model.Lines.Count, Is.EqualTo(2));

            Assert.That(model.Lines[0].ShortQuantity, Is.EqualTo(line1.ShortQuantity));
            Assert.That(model.Lines[1].ShortQuantity, Is.EqualTo(line2.ShortQuantity));
            Assert.That(model.Lines[0].JobDetailReason, Is.EqualTo(Enum<JobDetailReason>.GetDescription((JobDetailReason)line1.JobDetailReasonId)));
            Assert.That(model.Lines[1].JobDetailReason, Is.EqualTo(Enum<JobDetailReason>.GetDescription((JobDetailReason)line2.JobDetailReasonId)));
            Assert.That(model.Lines[0].JobDetailSource, Is.EqualTo(Enum<JobDetailSource>.GetDescription((JobDetailSource)line1.JobDetailSourceId)));
            Assert.That(model.Lines[1].JobDetailSource, Is.EqualTo(Enum<JobDetailSource>.GetDescription((JobDetailSource)line2.JobDetailSourceId)));
            Assert.That(model.Lines[0].ShortsAction, Is.EqualTo(Enum<DeliveryAction>.GetDescription((DeliveryAction)line1.ShortsActionId)));
            Assert.That(model.Lines[1].ShortsAction, Is.EqualTo(Enum<DeliveryAction>.GetDescription((DeliveryAction)line2.ShortsActionId)));
        }
    }
}