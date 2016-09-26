namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;

    using PH.Well.Domain.Enums;

    public class CreditThreshold : Entity<int>
    {
        public CreditThreshold()
        {
            this.Branches = new Collection<Branch>();
        }

        public UserRole UserRole => (UserRole)this.UserRoleId;

        public int UserRoleId { get; set; }

        public int Threshold { get; set; }

        public Collection<Branch> Branches { get; set; }
    }
}