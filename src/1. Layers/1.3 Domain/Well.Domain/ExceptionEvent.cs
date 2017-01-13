namespace PH.Well.Domain
{
    using System;

    using PH.Well.Domain.Enums;

    public class ExceptionEvent : Entity<int>
    {
        public string Event { get; set; }

        public EventAction EventAction => (EventAction)this.ExceptionActionId;

        public int ExceptionActionId { get; set; }

        public bool Processed { get; set; }

        public DateTime DateCanBeProcessed { get; set; }
    }
}