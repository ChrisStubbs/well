namespace PH.Well.Domain
{
    public class Branch : Entity<int>
    {
        public string Name { get; set; }

        public int PreferenceId { get; set; }
    }
}