namespace PH.Well.Repositories.Contracts
{
    public interface IDbConfiguration
    {
        string DatabaseConnection { get; }
        int TransactionTimeout { get; }
        int MaxNoOfDeadlockRetries { get; }
        int? CommandTimeout { get; }
    }
}
