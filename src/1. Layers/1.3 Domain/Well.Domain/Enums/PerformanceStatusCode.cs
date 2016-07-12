namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum PerformanceStatusCode
    {
        [Description("Not Departed")]
        Ndepa = 1,

        [Description("Early")]
        Early = 2,

        [Description("Late")]
        Late = 3,

        [Description("On Time")]
        Ontim = 3,

        [Description("Out of Sequence")]
        Outof = 3,
    }
}
