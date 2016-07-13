namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum RouteStatusCode
    {
        [Description("Not Departed")]
        NDEPA = 1,

        [Description("In Progress")]
        Inpro = 2,

        [Description("Complete")]
        Compl = 3,

        [Description("Not Defined")]
        Notdef = 4,
    }
}
