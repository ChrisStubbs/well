namespace PH.Well.Repositories.Contracts
{
    public interface IImportedFileRepository
    {
        bool HasFileAlreadyBeenImported(string importFileName);
    }
}