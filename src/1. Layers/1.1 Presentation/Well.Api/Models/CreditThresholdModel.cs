namespace PH.Well.Api.Models
{
    using System.Collections.ObjectModel;

    using PH.Well.Domain;

    public class CreditThresholdModel
    {
        public CreditThresholdModel()
        {
            this.Branches = new Collection<Branch>();
        }

        public int Id { get; set; }

        public string ThresholdLevel { get; set; }

        public int Threshold { get; set; }

        public string BranchName { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}