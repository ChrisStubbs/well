namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class UserStatsRepository : IUserStatsRepository
    {
        private readonly IDapperProxy dapperProxy;

        public UserStatsRepository(IDapperProxy dapperProxy)
        {
            this.dapperProxy = dapperProxy;
        }

        public UserStats GetByUser(string userIdentity)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.UserStatsGet)
                .AddParameter("UserIdentity", userIdentity, DbType.String)
                .Query<UserStats>()
                .SingleOrDefault();
        }
    }
}