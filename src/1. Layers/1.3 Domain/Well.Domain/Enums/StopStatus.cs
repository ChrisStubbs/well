namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum StopStatus
    {
        [Description("Not Arrived")]
        Narri = 1,

        [Description("Arrived")]
        Arriv = 2,

        [Description("Departed")]
        Depar = 3,

        [Description("Not Defined")]
        Notdef = 4,

    }
}
