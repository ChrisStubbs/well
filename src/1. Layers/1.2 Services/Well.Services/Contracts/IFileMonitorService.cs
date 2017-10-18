namespace PH.Well.Services.Contracts
{
    public interface IFileMonitorService
    {
        void Monitor(IAdamFileMonitorServiceConfig config);
    }
}
