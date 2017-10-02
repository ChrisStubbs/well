namespace PH.Well.Api.Models
{
    using System;
    using System.Collections.ObjectModel;

    using PH.Well.Domain;

    public class SeasonalDateModel
    {
        public SeasonalDateModel()
        {
            this.Branches = new Collection<Branch>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string BranchName { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}