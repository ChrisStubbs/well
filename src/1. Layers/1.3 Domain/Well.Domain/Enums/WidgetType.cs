namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum WidgetType
    {
        [Description("Exceptions")]
        Exceptions = 1,
        [Description("Outstanding")]
        Outstanding = 2,
        [Description("Assigned")]
        Assigned = 3,
        [Description("Notifications")]
        Notifications = 4
    }
}
