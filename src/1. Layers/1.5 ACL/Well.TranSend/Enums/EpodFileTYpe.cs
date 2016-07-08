namespace PH.Well.TranSend.Enums
{
    using System.ComponentModel;

    public enum EpodFileType
    {
        [Description("PH_")]
        RouteHeader = 1,

        [Description("ePOD_")]
        RouteEpod = 2,
    }
}
