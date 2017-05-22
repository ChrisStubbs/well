namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class BulkActionResult
    {
        public int JobDetailActionId { get; set; }
        public string Message { get; set; }
        public BulkActionResultType Type { get; set; }
        public bool CanAction { get; set; }
        public bool Actioned { get; set; }
    }

}