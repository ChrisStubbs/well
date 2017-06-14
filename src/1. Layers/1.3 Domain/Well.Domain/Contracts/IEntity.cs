namespace PH.Well.Domain.Contracts
{
    using System;

    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        DateTime DateCreated { get; }

        DateTime DateUpdated { get; }

        string CreatedBy { get; }

        string UpdatedBy { get; }

        DateTime? DateDeleted { get; }

        byte[] Version { get; set; }

        void SetDeletedProperties(string updatedBy);

        void SetCreatedProperties(string createdBy);

        void SetUpdatedProperties(string updatedBy);

        /// <summary>
        /// Check if this entity is transient, ie, without identity at this moment
        /// </summary>
        /// <returns>True if entity is transient, else false</returns>
        bool IsTransient();

        /// <summary>
        /// Helper to maintain backward compatibility
        /// Entry is deleted if the DateDeleted has a value (i.e. not null)
        /// </summary>
        bool IsDeleted { get; }
    }
}
