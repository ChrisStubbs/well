namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class WidgetModel
    {
        public WidgetModel()
        {
            Links = new List<WidgetLinkModel>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int WarningLevel { get; set; }
        public int SortOrder { get; set; }
        public bool ShowOnGraph { get; set; }
        public List<WidgetLinkModel> Links { get; set; }

        public bool ShowWarning => Count >= WarningLevel;
        public int Count => Links.Sum(l => l.Count);
    }
}