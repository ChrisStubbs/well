namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum WidgetType
    {
        [Description("Exceptions")]
        Exceptions = 1,
        [Description("Assigned")]
        Outstanding = 2,
        [Description("Outstanding")]
        Assigned = 3,
        [Description("Notifications")]
        Notifications = 4
    }
}
