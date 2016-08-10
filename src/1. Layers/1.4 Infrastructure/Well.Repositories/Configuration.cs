namespace PH.Well.Repositories
{
    using System.Configuration;

    public struct Configuration
    {
        public static int TransactionTimeout = int.Parse(ConfigurationManager.AppSettings["transactionTimeout"]);
        public static int WaitTimeInMillisecondsForFileToBeCopied => int.Parse(ConfigurationManager.AppSettings["waitTimeInMillisecondsForFileToBeCopied"]);
    }
}