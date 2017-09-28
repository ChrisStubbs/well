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

        Unknown = 4
    }
}
