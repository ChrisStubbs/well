namespace PH.Well.Repositories
{
    using Contracts;
    public class WellDapperProxy : BaseDapperProxy, IWellDapperProxy
    {
        public WellDapperProxy(IWellDbConfiguration wellDbConfiguration)
        {
            DbConfiguration = wellDbConfiguration;
        }
    }
}
