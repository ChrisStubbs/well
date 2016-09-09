namespace PH.Well.Domain.ValueObjects
{
    public class UserStats
    {
        public int ExceptionCount { get; set; }
        public int AssignedCount { get; set; }
        public int OutstandingCount { get; set; }
        public int NotificationsCount { get; set; }
    }
}