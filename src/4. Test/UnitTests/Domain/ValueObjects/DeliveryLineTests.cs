namespace PH.Well.UnitTests.Domain.ValueObjects
{
    using NUnit.Framework;

    using PH.Well.Domain.ValueObjects;
    using Well.Domain.Enums;

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

            [Test]
            public void IfNoDamageOrShortButInvoiceNotEqualToDeliverdTheNotClean()
            {
                var deliveryLine = new DeliveryLine { ShortQuantity = 0, InvoicedQuantity = 3, DeliveredQuantity = 2 };
                Assert.IsFalse(deliveryLine.IsClean);
            }

        }

        public class TheCanSubmitProperty : DeliveryLineTests
        {
            [Test]
            public void GivenShortActionUndefined_ThenCanNOTSubmit()
            {
                var deliveryLine = new DeliveryLine { ShortsActionId = (int)DeliveryAction.NotDefined, ShortQuantity = 1 };
                Assert.IsFalse(deliveryLine.CanSubmit);
            }

            [Test]
            public void GivenDamageActionUndefined_ThenCanNOTSubmit()
            {
                var deliveryLine = new DeliveryLine { ShortsActionId = (int)DeliveryAction.Credit, ShortQuantity = 1 };

                var damage1 = new Damage { DamageActionId = (int)DeliveryAction.NotDefined, Quantity = 1 };
                var damage2 = new Damage { DamageActionId = (int)DeliveryAction.Credit, Quantity = 1 };
                deliveryLine.Damages.Add(damage1);
                deliveryLine.Damages.Add(damage2);

                Assert.IsFalse(deliveryLine.CanSubmit);
            }

            [Test]
            public void GivenActionsDefined_ThenCANSubmit()
            {
                var deliveryLine = new DeliveryLine { ShortsActionId = (int)DeliveryAction.Credit, ShortQuantity = 1 };

                var damage1 = new Damage { DamageActionId = (int)DeliveryAction.Close, Quantity = 1 };
                var damage2 = new Damage { DamageActionId = (int)DeliveryAction.Credit, Quantity = 1 };
                deliveryLine.Damages.Add(damage1);
                deliveryLine.Damages.Add(damage2);

                Assert.IsTrue(deliveryLine.CanSubmit);
            }
        }
    }
}
