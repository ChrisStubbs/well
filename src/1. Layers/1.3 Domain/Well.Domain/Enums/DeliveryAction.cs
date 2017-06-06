namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum DeliveryAction
    {
        [Description("Not Defined")]
        NotDefined = 0,

        [Description("Credit")]
        Credit = 1,

        [Description("Mark as Bypassed")]
        MarkAsBypassed = 2,

        [Description("Mark as Delivered")]
        MarkAsDelivered = 3,

    }
}