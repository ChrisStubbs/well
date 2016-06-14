namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum JobType
    {
        [Description("Ad Hoc Collection")]
        Adhoc = 1,

        [Description("Collection")]
        Col = 2,

        [Description("Store Delivery Collection")]
        Dcol = 3,

        [Description("Delivery")]
        Del = 4,

        [Description("House Move Collection")]
        Hmc = 5,

        [Description("House Move Delivery")]
        Hmd = 6,

        [Description("Loan Collect")]
        Lc = 7,

        [Description("Loan Delivery")]
        Loan = 8,

        [Description("Re Delivery")]
        Redel = 9,

        [Description("Service Call")]
        Sc = 10,

        [Description("Showroom Delivery")]
        Sd = 11,

        [Description("Service Return")]
        Sr = 12,

    }
}
