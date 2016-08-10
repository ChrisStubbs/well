namespace PH.Well.Services.Contracts
{
    public interface IFileService
    {
        void WaitForFile(string filePath);

        void Reject(string filePath);

        void Archive(string filePath);
    }
}
