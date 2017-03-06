namespace PH.Well.UnitTests.Domain.ValueObjects
{
    using NUnit.Framework;

    using PH.Well.Domain.ValueObjects;

    [TestFixture]
    public class DeliveryTests
    {
        public class TheSetCanActionMethod : DeliveryTests
        {
            [Test]
            public void ShouldSetCanActionToTrueWhenNamesMatch()
            {
                var delivery = new Delivery { IdentityName = "foo", CurrentUserIdentity = "foo"};
                Assert.IsTrue(delivery.CanAction);
            }

            [Test]
            public void ShouldSetCanActionToFalseWhenNamesDONTMatch()
            {
                var delivery = new Delivery { IdentityName = "bar", CurrentUserIdentity = "foo"};
                Assert.IsFalse(delivery.CanAction);
            }
        }
    }
}