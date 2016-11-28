namespace PH.Well.Services.Contracts
{
    using System.IO;

    public interface IFileModule
    {
        FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);

        void Move(string sourceFilePath, string newFilePath);

        void MoveFile(string filename, string location);
    }
}
