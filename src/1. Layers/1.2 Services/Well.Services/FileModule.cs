namespace PH.Well.Services
{
    using System.IO;

    using PH.Well.Services.Contracts;

    //to unit tests this we could use NuGet package System.IO.Abstractions
    public class FileModule : IFileModule
    {
        public FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
           return File.Open(path, mode, access, share);
        }

        public void Move(string sourceFilePath, string newFilePath)
        {
           File.Move(sourceFilePath, Path.Combine(newFilePath, Path.GetFileName(sourceFilePath)));
        }

        public void MoveFile(string filename, string location)
        {
            this.CreateDirectory(location);

            var newFilename = Path.Combine(location, Path.GetFileName(filename));

            if (File.Exists(newFilename))
            {
                File.Delete(newFilename);
            }

            File.Move(filename, newFilename);
        }

        private void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
