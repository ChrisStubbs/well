namespace PH.Well.Domain
{
    public class DateThreshold : Entity<int>
    {
        public int BranchId { get; set; }
        public string Branch { get; set; }
        public byte NumberOfDays { get; set; }
    }
}
