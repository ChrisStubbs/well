namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum ExceptionType
    {
        [Description("Short")]
        Short = 1,

        [Description("Bypass")]
        Bypass = 2,

        [Description("Damage")]
        Damage = 3
    }
}
