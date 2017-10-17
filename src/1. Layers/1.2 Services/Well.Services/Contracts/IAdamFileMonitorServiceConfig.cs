namespace PH.Well.Services.Contracts
{
    public interface IAdamFileMonitorServiceConfig
    {
        /// <summary>
        /// Location to monitor
        /// </summary>
        string RootFolder { get; }

        /// <summary>
        /// Whether should process files before archiving
        /// </summary>
        bool ProcessFiles { get; }
    }
}