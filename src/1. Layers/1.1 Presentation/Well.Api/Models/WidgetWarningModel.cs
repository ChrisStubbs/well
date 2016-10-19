namespace PH.Well.Api.Models
{
    using System.Collections.ObjectModel;
    using Domain;

    public class WidgetWarningModel
    {
        public WidgetWarningModel()
        {
            this.Branches = new Collection<Branch>();
        }

        public int Id { get; set; }

        public string WidgetName { get; set; }

        public int? WarningLevel { get; set; }

        public string BranchName { get; set; }

        public Collection<Branch> Branches { get; set; }

        public string Type { get; set; }
    }
}