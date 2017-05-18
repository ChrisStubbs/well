namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum DeliveryAction
    {
        [Description("Not Defined")]
        NotDefined = 0,

        [Description("Credit")]
        Credit = 1,

        [Description("Reject")]
        Reject = 3,

        [Description("Close")]
        Close = 4,
    }
}