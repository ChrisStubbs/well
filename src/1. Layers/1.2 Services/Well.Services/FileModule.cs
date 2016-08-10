namespace PH.Well.Services
{
    using System.IO;

    using PH.Well.Services.Contracts;

    public class FileModule : IFileModule
    {
        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
          {
              return File.Open(path, mode, access, share);
          }

        public void Move(string sourceFilePath, string newFilePath)
          {
              File.Move(sourceFilePath, newFilePath);
          }
    }
}
