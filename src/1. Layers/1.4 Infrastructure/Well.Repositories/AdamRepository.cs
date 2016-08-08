namespace PH.Well.Repositories
{
    using AIA.Adam.RFS;
    using AIA.ADAM.DataProvider;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class AdamRepository : IAdamRepository
    {
        public AdamResponse CreditInvoice(CreditEvent credit, AdamSettings adamSettings)
        {
            using (var connection = new AdamConnection(GetConnection(adamSettings)))
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

        private static string GetConnection(AdamSettings settings)
        {
            var connection = new AdamConnectionStringBuilder
            {
                Pooling = true,
                MinPoolSize = 1,
                MaxPoolSize = 32,
                ConnectTimeout = 60,
                TransactionMode = AdamTransaction.TransactionMode.Ignore,
                DataSource = settings.Server,
                Database = settings.Rfs,
                Port = settings.Port,
                UID = settings.Username,
                PWD = settings.Password,
                OpenMode = AdamOpenMode.NonexclusiveReadWrite
            };

            return connection.ConnectionString;
        }
    }
}