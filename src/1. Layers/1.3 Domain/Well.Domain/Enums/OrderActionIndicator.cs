namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum OrderActionIndicator
    {
        [Description("A")]
        InsertOrUpdate = 1,

        [Description("I")]
        InsertOnly = 2,

        [Description("D")]
        Delete = 3
    }
}
