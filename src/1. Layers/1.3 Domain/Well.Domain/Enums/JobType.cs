namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum JobType
    {
        [Description("DEL-TOB")]
        DelTob = 1,

        [Description("DEL-AMB")]
        DelAmb = 2,

        [Description("DEL-ALC")]
        DelAlc = 3,

        [Description("DEL-CF")]
        DelFc = 4,

        [Description("DEL-DOC")]
        DelDoc = 5,

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
