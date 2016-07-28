namespace PH.Well.Adam.Contracts
{
    public interface IAdamImportConfiguration
    {
        string FilePath { get; set; }
        string SearchPattern { get; set; }
        string ArchiveLocation { get; set; }
    }
}
