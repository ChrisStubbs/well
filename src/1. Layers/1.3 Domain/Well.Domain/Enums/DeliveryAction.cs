namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum DeliveryAction
    {
        [Description("Not Defined")]
        NotDefined = 0,

        [Description("Credit")]
        Credit = 1,

        [Description("Close")]
        Close = 2,

        [Description("POD")]
        Pod = 3,

    }
}