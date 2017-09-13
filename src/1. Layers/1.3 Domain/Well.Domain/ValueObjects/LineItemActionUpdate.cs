namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class LineItemActionUpdate
    {
        public int Id { get; set; }
        public int LineItemId { get; set; }
        public DeliveryAction DeliverAction { get; set; }
        public ExceptionType ExceptionType { get; set; }
        public int Quantity { get; set; }
        public JobDetailSource Source { get; set; }
        public JobDetailReason Reason { get; set; }
        public Originator Orginator { get; set; }
    }
}
