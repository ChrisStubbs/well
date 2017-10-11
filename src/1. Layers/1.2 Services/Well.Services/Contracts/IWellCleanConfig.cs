namespace PH.Well.Services.Contracts
{
    public interface IWellCleanConfig
    {
        int SoftDeleteBatchSize { get; set; }
        int WellCleanTransactionTimeoutSeconds { get; set; }
    }
}