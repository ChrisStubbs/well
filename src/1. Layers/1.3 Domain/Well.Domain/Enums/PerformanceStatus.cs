namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum PerformanceStatus
    {
        [Description("Not Arrived")]
        Narri = 1,

        [Description("Not Done")]
        Ndone = 2,

        [Description("Authorised ByPass")]
        Abypa = 3,

        [Description("Non Authorised ByPass")]
        Nbypa = 4,

        [Description("Incomplete")]
        Incom = 5,

        [Description("Complete")]
        Compl = 6,

        [Description("Not Defined")]
        Notdef = 7,

        [Description("Resolved")]
        Resolved = 8,

        [Description("Pending Authorisation")]
        PendingAuthorisation = 9
    }
}
