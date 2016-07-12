namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum RouteStatusCode
    {
        [Description("Not Departed")]
        Ndepa = 1,

        [Description("In Progress")]
        Inpro = 2,

        [Description("Complete")]
        Compl = 3,
    }
}
