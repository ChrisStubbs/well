namespace PH.Well.Common.Contracts
{
    using System;
    using System.Net;

    public interface IApiClient
    {
        bool UseDefaultCredentials { get; set; }

        ICredentials Credentials { get; set; }

        WebProxy Proxy { get; set; }

        WebHeaderCollection Headers { get; set; }

        HttpWebResponse HttpWebResponse { get; set; }

        string UploadString(string address, string method, string data);

        string UploadString(string address, string method, string data, int timeout);

        string DownloadString(string address);

        string DownloadString(string address, int timeout);

        void DownloadFile(string address, string fileName);

        void DownloadFile(Uri address, string fileName);
    }
}
