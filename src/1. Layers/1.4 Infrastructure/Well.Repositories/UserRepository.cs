namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class UserRepository : DapperRepository<User, int>, IUserRepository
    {
        public UserRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public User GetByName(string name)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.UserGetByName)
                    .AddParameter("Name", name, DbType.String, size: 500)
                    .Query<User>()
                    .SingleOrDefault();
        }

        protected override void SaveNew(User entity)
        {
            entity.Id =
                this.dapperProxy.WithStoredProcedure(StoredProcedures.UserSave)
                    .AddParameter("Name", entity.Name, DbType.String, size: 500)
                    .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                    .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                    .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
                    .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                    .Query<int>().SingleOrDefault();
        }
    }
}