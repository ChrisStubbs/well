namespace PH.Well.TranSend.Contracts
{
    public interface IEpodImportConfiguration
    {
        string ArchiveLocation { get; set; }
        string FilePath { get; set; }
        string FtpLocation { get; set; }
        string FtpPass { get; set; }
        string FtpUser { get; set; }
        string NetworkUser { get; set; }
        string NetworkUserPass { get; set; }
        string SearchPattern { get; set; }
        string DashboardRefreshEndpoint { get; }
    }
}