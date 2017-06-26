namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain.Contracts;

    public interface IRepository<TEntity, in TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        int? TotalRecords { get; }

        void Save(TEntity entity);

        void Update(TEntity entity);

        void BulkCopyInsert(IEnumerable<TEntity> entities, string tableName, IEnumerable<string> mappings);

        string CurrentUser { get; }
    }
}
