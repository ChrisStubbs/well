namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    using WebGrease.Css.Extensions;

    public class CleanPreferenceRepository : DapperRepository<CleanPreference, int>, ICleanPreferenceRepository
    {
        public CleanPreferenceRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public IEnumerable<CleanPreference> GetAll()
        {
            var cleanPreferences = this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferencesGetAll).Query<CleanPreference>();

            foreach (var cleanPreference in cleanPreferences)
            {
                var branches = this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferencesBranchesGet)
                    .AddParameter("cleanPreferenceId", cleanPreference.Id, DbType.Int32).Query<Branch>();

                branches.ForEach(x => cleanPreference.Branches.Add(x));
            }

            return cleanPreferences;
        }
    }
}