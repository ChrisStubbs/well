namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;
    using PH.Well.Domain.Extensions;

    public enum Originator
    {
        [Description("Driver")]
        Driver = 0,

        [Description("Customer")]
        Customer = 1,
    }

    public static class OriginatorDescriptions
    {
        private static readonly string driver;
        private static readonly string customer;

        static OriginatorDescriptions()
        {
            customer = EnumExtensions.GetDescription(Originator.Customer);
            driver = EnumExtensions.GetDescription(Originator.Driver);
        }

        public static string Description(this Originator value)
        {
            switch (value)
            {
                case Originator.Driver:
                    return driver;

                case Originator.Customer:
                    return customer;

                default:
                    return null;
            }
        }
    }
}
