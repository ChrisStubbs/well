namespace PH.Well.Api.Models
{
    public class WidgetModel
    {
        public string Name { get; set; }
        public string LinkText { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int WarningLevel { get; set; }
        public int SortOrder { get; set; }
        public bool ShowOnGraph { get; set; }
        public bool ShowWarning => Count >= WarningLevel;
    }
}