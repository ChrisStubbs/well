namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum Originator
    {
        [Description("Driver")]
        Driver = 0,

        [Description("Customer")]
        Customer = 1,
    }
}
