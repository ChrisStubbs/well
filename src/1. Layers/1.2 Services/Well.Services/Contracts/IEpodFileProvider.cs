namespace PH.Well.Services.Contracts
{
    public interface IEpodFileProvider
    {
        bool TryImport(string filePath, string filename, IImportConfig config);
    }
}