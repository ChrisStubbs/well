namespace PH.Well.Services
{
    using System.IO;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Services.Contracts;

    public class MonitorService : IMonitorService
{
    private readonly ILogger logger;

    private readonly IFileService fileService;


    public MonitorService(ILogger logger, IFileService fileService)
    {
        this.logger = logger;
        this.fileService = fileService;
    }

    public void Monitor(string rootFolder)
    {
        var watcher = new FileSystemWatcher
        {
            Path = rootFolder,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.LastAccess,
            IncludeSubdirectories = true
        };

        watcher.Created += this.ProcessCreated;
        // watcher.Renamed += this.ProcessRenamed;
        // watcher.Deleted += this.ProcessDeleted;

        watcher.EnableRaisingEvents = true;
    }

    private void ProcessCreated(object o, FileSystemEventArgs args)
    {
        // ignore archive and rejected folders
        if (args.FullPath.Contains("archive") || args.FullPath.Contains("rejected")) return;

        this.logger.LogDebug($"File created ({args.FullPath})");
        this.fileService.WaitForFile(args.FullPath);


    }

    /*private void ProcessDeleted(object o, FileSystemEventArgs args)
    {
        this.logger.LogDebug($"File deleted ({args.FullPath})");
    }

    private void ProcessRenamed(object o, RenamedEventArgs args)
    {
        this.logger.LogDebug($"File renamed ({args.FullPath})");
    }*/
}
}
