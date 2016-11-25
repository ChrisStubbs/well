namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum EpodFileType
    {
        [Description("ROUTE_")]
        AdamInsert = 1,

        [Description("ePOD_")]
        EpodUpdate = 2,

        [Description("Order_")]
        AdamUpdate = 3,

        Unknown = 4
    }
}
