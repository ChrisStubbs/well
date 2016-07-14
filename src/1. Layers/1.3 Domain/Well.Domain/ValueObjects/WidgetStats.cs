namespace PH.Well.Domain.ValueObjects
{
    public class WidgetStats: Entity<int>
    {
        public int NoOfExceptions { get; set; }
        public int Assigned { get; set; }
        public int Outstanding { get; set; }
        public int OnHold { get; set; }
        public int Notifications { get; set; }
    }
}
