namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;
    using Enums;

    public class WidgetWarning : Entity<int>
    {
        public WidgetWarning()
        {
             this.Branches = new Collection<Branch>();
        }

        public string WidgetName { get; set; }

        public int WarningLevel { get; set; }

        public Collection<Branch> Branches { get; set; }

        public WidgetType WidgetType => (WidgetType)this.Type;

        public int Type { get; set; }
    }
}
