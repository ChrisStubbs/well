namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services;

    [TestFixture]
    public class DeliveryLineToDeliveryLineCreditMapperTests
    {
        private DeliverLineToDeliveryLineCreditMapper mapper;

        [SetUp]
        public void Setup()
        {
            this.mapper = new DeliverLineToDeliveryLineCreditMapper();
        }

        [Test]
        public void ShouldMap()
        {
            var deliveryLines = new List<DeliveryLine>();

            var line1 = new DeliveryLine
            {
                ShortQuantity = 1,
                JobId = 4,
                JobDetailReasonId = 3,
                JobDetailSourceId = 5,
                ProductCode = "3222"
            };

            var damage1 = new Damage { Quantity = 4, JobDetailReasonId = 3, JobDetailSourceId = 5, DamageActionId = (int)DeliveryAction.Credit};

            line1.Damages.Add(damage1);

            deliveryLines.Add(line1);

            var credits = this.mapper.Map(deliveryLines);

            Assert.That(credits.Count, Is.EqualTo(2));

            var credit1 = credits[0];
            var credit2 = credits[1];

            Assert.That(credit1.JobId, Is.EqualTo(line1.JobId));
            Assert.That(credit1.Quantity, Is.EqualTo(line1.ShortQuantity));
            Assert.That(credit1.Reason, Is.EqualTo(line1.JobDetailReasonId));
            Assert.That(credit1.Source, Is.EqualTo(line1.JobDetailSourceId));
            Assert.That(credit1.ProductCode, Is.EqualTo(line1.ProductCode));

            Assert.That(credit2.JobId, Is.EqualTo(line1.JobId));
            Assert.That(credit2.Quantity, Is.EqualTo(damage1.Quantity));
            Assert.That(credit2.Reason, Is.EqualTo(damage1.JobDetailReasonId));
            Assert.That(credit2.Source, Is.EqualTo(damage1.JobDetailSourceId));
            Assert.That(credit2.ProductCode, Is.EqualTo(line1.ProductCode));
        }
    }
}