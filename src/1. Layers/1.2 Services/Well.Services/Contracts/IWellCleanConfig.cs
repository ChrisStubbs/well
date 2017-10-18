namespace PH.Well.Services.Contracts
{
    public interface IWellCleanConfig
    {
        int CleanBatchSize { get; set; }
        int WellCleanTransactionTimeoutSeconds { get; set; }
    }
}