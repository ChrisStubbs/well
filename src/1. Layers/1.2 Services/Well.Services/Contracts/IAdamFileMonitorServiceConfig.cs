namespace PH.Well.Services.Contracts
{
    public interface IAdamFileMonitorServiceConfig : IImportConfig
    {
        /// <summary>
        /// Location to monitor
        /// </summary>
        string RootFolder { get; }
    }
}