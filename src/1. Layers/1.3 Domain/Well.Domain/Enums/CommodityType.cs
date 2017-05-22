namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum CommodityType
    {
        [Description("Alcohol Outers")]
        A = 1,

        [Description("Alcohol Singles")]
        B = 2,

        [Description("Confectionary")]
        C = 3,

        [Description("Frozen")]
        F = 4,

        [Description("Chilled")]
        G = 5,

        [Description("Impulsive")]
        I = 6,

        [Description("Central")]
        J = 7,

        [Description("Central Singles")]
        K = 8,

        [Description("Confectionary Singles")]
        O = 9,

        [Description("Phone Cards")]
        P = 10,

        [Description("Tobabcco")]
        T = 11,

        [Description("Hub Tobabcco")]
        U = 12
    }
}
