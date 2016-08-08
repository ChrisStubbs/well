namespace PH.Well.Domain
{
    using PH.Well.Domain.Enums;

    public class ExceptionEvent : Entity<int>
    {
        public string Event { get; set; }

        public ExceptionAction ExceptionAction => (ExceptionAction)this.ExceptionActionId;

        public int ExceptionActionId { get; set; }

        public bool Processed { get; set; }
    }
}