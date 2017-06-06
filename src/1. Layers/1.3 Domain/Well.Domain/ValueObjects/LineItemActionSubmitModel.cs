namespace PH.Well.Domain.ValueObjects
{
    public class LineItemActionSubmitModel : LineItemAction
    {
        public int JobId { get; set; }
        public int BranchId { get; set; }
        public string ProductCode { get; set; }
    }
}