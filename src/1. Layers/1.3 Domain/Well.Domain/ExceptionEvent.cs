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

        /// <summary>
        /// Optional Id. 
        /// Can be used to store implicit relation key.
        /// Client needs to know related entities and how to query them
        /// </summary>
        public string SourceId { get; set; }

        public DateTime DateCanBeProcessed { get; set; }
    }
}