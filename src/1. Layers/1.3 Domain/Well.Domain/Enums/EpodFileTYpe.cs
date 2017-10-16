namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum EpodFileType
    {
        [Description("ROUTE_")]
        Route = 1,

        [Description("ePOD_")]
        Epod = 2,

        [Description("Order_")]
        Order = 3,

        [Description("CLEAN_")]
        Clean = 4,

        Unknown = 5
    }
}
