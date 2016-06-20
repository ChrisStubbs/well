namespace PH.Well.Repositories
{
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class PreferenceRepository : DapperRepository<Preference, int>, IPreferenceRepository
    {
        public PreferenceRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }
    }
}