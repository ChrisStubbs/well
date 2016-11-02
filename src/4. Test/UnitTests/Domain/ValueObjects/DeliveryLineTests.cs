namespace PH.Well.UnitTests.Domain.ValueObjects
{
    using NUnit.Framework;

    using PH.Well.Domain.ValueObjects;

    [TestFixture]
    public class DeliveryLineTests
    {
        public class TheDamagedQuantityProperty : DeliveryLineTests
        {
            [Test]
            public void ShouldSumTheQuantityOfDamages()
            {
                var deliveryLine = new DeliveryLine();

                var damage1 = new Damage { Quantity = 5 };
                var damage2 = new Damage { Quantity = 2 };

                deliveryLine.Damages.Add(damage1);
                deliveryLine.Damages.Add(damage2);

                Assert.That(deliveryLine.DamagedQuantity, Is.EqualTo(7));
            }
        }

        public class TheDeliveredQuantityProperty : DeliveryLineTests
        {
            [Test]
            public void ShouldCalculateTheDeliveredQuantity()
            {
                var deliveryLine = new DeliveryLine { InvoicedQuantity = 20, ShortQuantity = 3 };

                var damage1 = new Damage { Quantity = 5 };
                var damage2 = new Damage { Quantity = 2 };

                deliveryLine.Damages.Add(damage1);
                deliveryLine.Damages.Add(damage2);

                Assert.That(deliveryLine.DeliveredQuantity, Is.EqualTo(10));
            }
        }

        public class TheIsCleanProperty : DeliveryLineTests
        {
            [Test]
            public void ShouldDetermineTheLineIsNotClean()
            {
                var deliveryLine = new DeliveryLine { ShortQuantity = 3 };

                var damage1 = new Damage { Quantity = 5 };
                var damage2 = new Damage { Quantity = 2 };

                deliveryLine.Damages.Add(damage1);
                deliveryLine.Damages.Add(damage2);

                Assert.IsFalse(deliveryLine.IsClean);
            }

            [Test]
            public void ShouldDetermineTheLineIsClean()
            {
                var deliveryLine = new DeliveryLine { ShortQuantity = 0 };

                var damage1 = new Damage { Quantity = 0 };
                var damage2 = new Damage { Quantity = 0 };

                deliveryLine.Damages.Add(damage1);
                deliveryLine.Damages.Add(damage2);

                Assert.IsTrue(deliveryLine.IsClean);
            }
        }
    }
}