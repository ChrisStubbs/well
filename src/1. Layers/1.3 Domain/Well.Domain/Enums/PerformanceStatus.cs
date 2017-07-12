namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum PerformanceStatus
    {
        /// <summary>
        /// Not Arrived
        /// </summary>
        [Description("Not Arrived")]
        Narri = 1,

        /// <summary>
        /// Not Done
        /// </summary>
        [Description("Not Done")]
        Ndone = 2,

        /// <summary>
        /// Authorised ByPass
        /// </summary>
        [Description("Authorised ByPass")]
        Abypa = 3,

        /// <summary>
        /// Non Authorised ByPass
        /// </summary>
        [Description("Non Authorised ByPass")]
        Nbypa = 4,

        /// <summary>
        /// Incomplete
        /// </summary>
        [Description("Incomplete")]
        Incom = 5,

        /// <summary>
        /// Complete
        /// </summary>
        [Description("Complete")]
        Compl = 6,

        /// <summary>
        /// Not Defined
        /// </summary>
        [Description("Not Defined")]
        Notdef = 0,

        /// <summary>
        /// Resolved
        /// </summary>
        [Description("Resolved")]
        Resolved = 7,

        /// <summary>
        /// Pending Authorisation
        /// </summary>
        [Description("Pending Authorisation")]
        PendingAuthorisation = 8,

        /// <summary>
        /// Submitted
        /// </summary>
        [Description("Submitted")]
        Submitted = 9,

        /// <summary>
        /// Authorised ByPass
        /// </summary>
        [Description("Well ByPass")]
        Wbypa = 10,
    }
}
