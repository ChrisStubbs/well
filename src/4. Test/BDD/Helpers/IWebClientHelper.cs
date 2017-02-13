namespace PH.Well.BDD.Helpers
{
    using System.Net;

    public interface IWebClientHelper
    {
        string Get(string url);
        string Post(string url, string data);
        string UploadString(string url, string method, string data);
        string Put(string url, string data);
        string Patch(string url, string data);
        string UploadStringAndThrow(string url, string method, string data);
        HttpWebResponse HttpWebResponse { get; }
        string Response { get; set; }
    }
}