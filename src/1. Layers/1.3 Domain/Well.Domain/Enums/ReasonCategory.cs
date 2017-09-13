namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum ReasonCategory
    {
        [Description("Auth")]
        Auth = 1,

        [Description("DA")]
        Da = 2,

        [Description("DR")]
        Dr = 3
    }
}
