namespace PH.Well.Dashboard.Models
{
    using System.Collections.ObjectModel;

    using PH.Well.Domain;

    public class SeasonalDateModel
    {
        public SeasonalDateModel()
        {
            this.Branches = new Collection<Branch>();
        }

        public string Description { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}