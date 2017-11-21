namespace PH.Well.Common.Contracts
{
    using System.IO;

    public interface IFtpClient
    {
        string FtpLocation { get; set; }

        string FtpUserName { get; set; }

        string FtpPassword { get; set; }

        Stream GetResponseStream();

        void DeleteFile(string filename);
    }
}