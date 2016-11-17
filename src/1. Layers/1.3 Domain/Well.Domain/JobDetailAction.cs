namespace PH.Well.Domain
{
    using Enums;

    public class JobDetailAction : Entity<int>
    {
        public int JobDetailId { get; set; }

        public int Quantity { get; set; }

        public ExceptionAction Action { get; set; }

        public ActionStatus Status { get; set; }

        public string GetString()
        {
            return $"Action: {Action}, Quantity: {Quantity}, Status: {Status}";
        }
    }
}
