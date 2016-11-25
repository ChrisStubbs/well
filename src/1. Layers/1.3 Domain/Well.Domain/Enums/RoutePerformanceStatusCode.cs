namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum RoutePerformanceStatusCode
    {
        [Description("Not Departed")]
        Ndepa = 1,

        [Description("Early")]
        Early = 2,

        [Description("Late")]
        Late = 3,

        [Description("On Time")]
        Ontim = 4,

        [Description("Out of Sequence")]
        Outof = 5,

        [Description("Not Defined")]
        Notdef = 0,
    }
}
