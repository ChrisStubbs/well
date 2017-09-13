namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum OrderActionIndicator
    {
        [Description("U")]
        Update = 1,

        [Description("I")]
        Insert = 2,

        [Description("D")]
        Delete = 3
    }
}
