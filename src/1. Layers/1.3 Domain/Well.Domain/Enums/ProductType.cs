namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum ProductType
    {
        [Description("Tobacco")]
        Tob = 1,

        [Description("Chilled")]
        Chld = 2,

        [Description("Frozen")]
        Frzn = 3,

        [Description("Alcohol")]
        Alc = 4,

        [Description("Ambient")]
        Amb = 5,
    }
}
