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
        Notdef = 0,

        [Description("Resolved")]
        Resolved = 7,

        [Description("Pending Authorisation")]
        PendingAuthorisation = 8,

        [Description("Submitted")]
        Submitted = 9
    }
}
