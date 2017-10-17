namespace PH.Well.Services.Contracts
{
    public interface IEpodFileProvider
    {
        void Import(string filePath, string filename, IImportConfig config);
    }
}