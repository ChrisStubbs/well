namespace PH.Well.Domain
{
    public class Activity : Entity<int>
    {
        public string DocumentNumber { get; set; }
        public string InitialDocument { get; set; }
        public int ActivityTypeId { get; set; }
    }
}
