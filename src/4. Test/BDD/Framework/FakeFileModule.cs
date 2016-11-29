namespace PH.Well.BDD.Framework
{
    using System.IO;

    using PH.Well.Services.Contracts;

    public class FakeFileModule : IFileModule
    {
        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return File.Open(path, mode, access, share);
        }

        public void Move(string sourceFilePath, string newFilePath)
        {
            
        }

        public void MoveFile(string filename, string location)
        {
            
        }
    }
}