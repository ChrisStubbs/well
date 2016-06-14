namespace PH.Well.Domain.Base
{
    using System;
    using System.Collections.Generic;

    using PH.Well.Domain.Contracts;

    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public byte[] Version { get; set; }

        public void SetDeletedProperties(string updatedBy)
        {
            this.DateUpdated = DateTime.Now;
            this.UpdatedBy = updatedBy;
            this.IsDeleted = true;
        }

        public void SetCreatedProperties(string createdBy)
        {
            this.DateCreated = DateTime.Now;
            this.DateUpdated = DateTime.Now;
            this.CreatedBy = createdBy;
            this.UpdatedBy = createdBy;
        }

        public void SetUpdatedProperties(string updatedBy)
        {
            this.DateUpdated = DateTime.Now;
            this.UpdatedBy = updatedBy;
        }

        public bool IsTransient()
        {
            return EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey));
        }

        public override string ToString()
        {
            return $"[{this.GetType().Name} {this.Id}]";
        }
    }
}