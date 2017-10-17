using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class AdamFileMonitorServiceConfig : IAdamFileMonitorServiceConfig
    {
        public string RootFolder { get; }
        public bool ProcessFiles { get; }

        public AdamFileMonitorServiceConfig(string rootFolder, bool processFiles)
        {
            RootFolder = rootFolder;
            ProcessFiles = processFiles;
        }
    }
}