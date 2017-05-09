namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum JobType
    {
        Unknown = 0,

        [Description("DEL-TOB")]
        Tobacco = 1,

        [Description("DEL-AMB")]
        Ambient = 2,

        [Description("DEL-ALC")]
        Alcohol = 3,

        [Description("DEL-CHL")]
        Chilled = 4,

        [Description("DEL-FRZ")]
        Frozen = 5,

        [Description("DEL-DOC")]
        Documents = 6,

        [Description("UPL-SAN")]
        SandwichUplift = 7,

        [Description("UPL-GLO")]
        GlobalUplift = 8,

        [Description("UPL-ASS")]
        AssetsUplift = 9

    }
}
