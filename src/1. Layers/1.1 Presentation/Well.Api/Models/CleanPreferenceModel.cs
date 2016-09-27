namespace PH.Well.Api.Models
{
    using System.Collections.ObjectModel;

    using PH.Well.Domain;

    public class CleanPreferenceModel
    {
        public CleanPreferenceModel()
        {
            this.Branches = new Collection<Branch>();
        }

        public int Id { get; set; }

        public int Days { get; set; }

        public string BranchName { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}