namespace PH.Well.Api.Models
{
    using Domain.Enums;

    public class ActionModel
    {
        public int Quantity { get; set; }
        public ExceptionAction Action { get; set; }
        public ActionStatus Status { get; set; }
        public string StatusDescription => Status.ToString();
    }
}