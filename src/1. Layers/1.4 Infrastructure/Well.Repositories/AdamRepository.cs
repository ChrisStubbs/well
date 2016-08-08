namespace PH.Well.Repositories
{
    using AIA.Adam.RFS;
    using AIA.ADAM.DataProvider;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class AdamRepository : IAdamRepository
    {
        public AdamResponse CreditInvoice(CreditEvent credit, AdamConfiguration configuration)
        {
            using (var connection = new AdamConnection(GetConnection(configuration)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        command.CommandText = "INSERT INTO LEEFILE(LEETYPE, LEEBRANCH, LEEACCNO, LEEINVNO, LEENOTES) VALUES(1, 2, 3, 4, 'HI LEE');";
                        command.ExecuteNonQuery();
                    }

                    return AdamResponse.Success;
                }
                catch (AdamProviderException adamException)
                {
                    if (adamException.AdamErrorId == AdamError.ADAMNOTRUNNING)
                    {
                        return AdamResponse.AdamDown;
                    }
                }
            }

            return AdamResponse.Unknown;
        }

        private static string GetConnection(AdamConfiguration configuration)
        {
            var connection = new AdamConnectionStringBuilder
            {
                Pooling = true,
                MinPoolSize = 1,
                MaxPoolSize = 32,
                ConnectTimeout = 60,
                TransactionMode = AdamTransaction.TransactionMode.Ignore,
                DataSource = configuration.Server,
                Database = configuration.Rfs,
                Port = configuration.Port,
                UID = configuration.Username,
                PWD = configuration.Password,
                OpenMode = AdamOpenMode.NonexclusiveReadWrite
            };

            return connection.ConnectionString;
        }
    }
}