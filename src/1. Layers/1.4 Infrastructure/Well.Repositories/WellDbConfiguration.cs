namespace PH.Well.Repositories
{
    using System.Configuration;
    using System.Data;
    using Contracts;

    public class WellDbConfiguration : BaseDbConfiguration, IWellDbConfiguration
    {
        public string DatabaseConnection
        {
            get
            {
                ConnectionStringSettings wellConnectionString = ConfigurationManager.ConnectionStrings["Well"];

                if (wellConnectionString == null)
                {
                    throw new ConstraintException("Well ConnectionString not found");
                }

                return wellConnectionString.ConnectionString;
            }
        }
    }
}
