namespace PH.Well.Domain.ValueObjects
{
    public class JobDetailsWithAction
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public int Action { get; set; }
        public int Reason { get; set; }
        public int Source { get; set; }
        public int Quantity { get; set; }
    }
}
