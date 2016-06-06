namespace PH.Well.Common
{
    using System;
    using System.IO;
    using System.Net;
    using Common.Contracts;

    public class ApiClient : IApiClient
    {
        private System.Net.WebClient wc;

        public ApiClient()
        {
            this.Headers = new WebHeaderCollection();
        }

        public bool UseDefaultCredentials { get; set; }

        public ICredentials Credentials { get; set; }

        public WebProxy Proxy { get; set; }

        public WebHeaderCollection Headers { get; set; }

        public HttpWebResponse HttpWebResponse { get; set; }

        public string DownloadString(string address)
        {
            HttpWebResponse = GetHttpWebRequest(address, WebRequestMethods.Http.Get).GetResponse() as HttpWebResponse;
            return new StreamReader(HttpWebResponse.GetResponseStream()).ReadToEnd();
        }

        public string DownloadString(string address, int timeout)
        {
            HttpWebResponse = GetHttpWebRequest(address, WebRequestMethods.Http.Get, timeout).GetResponse() as HttpWebResponse;
            return new StreamReader(HttpWebResponse.GetResponseStream()).ReadToEnd();
        }

        public string UploadString(string address, string method, string data)
        {
            return DoUploadString(address, method, data, null);
        }

        public string UploadString(string address, string method, string data, int timeout)
        {
            return DoUploadString(address, method, data, timeout);
        }

        public void DownloadFile(string address, string fileName)
        {
            this.DownloadFile(new Uri(address), fileName);
        }

        public void DownloadFile(Uri address, string fileName)
        {
            wc = this.SetupClient();
            wc.DownloadFile(address, fileName);
        }

        private string DoUploadString(string address, string method, string data, int? timeout)
        {
            var httpWebRequest = GetHttpWebRequest(address, method, timeout);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            string result;
            HttpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(HttpWebResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        private HttpWebRequest GetHttpWebRequest(string url, string method, int? timeout = null)
        {
            HttpWebRequest.DefaultMaximumErrorResponseLength = 1048576;

            var httpWebRequest = WebRequest.Create(url) as HttpWebRequest;

            foreach (string key in Headers.AllKeys)
            {
                httpWebRequest.SetRawHeader(key, Headers[key]);
            }

            httpWebRequest.Method = method;
            httpWebRequest.UseDefaultCredentials = true;
            if (Credentials != null)
                httpWebRequest.Credentials = this.Credentials;

            if (timeout.HasValue)
                httpWebRequest.Timeout = timeout.Value;

            return httpWebRequest;
        }
        private System.Net.WebClient SetupClient()
        {
            HttpWebRequest.DefaultMaximumErrorResponseLength = 1048576;
            var webClient = new System.Net.WebClient { UseDefaultCredentials = this.UseDefaultCredentials };

            if (Credentials != null)
            {
                webClient.Credentials = Credentials;
            }

            if (this.Headers != null)
            {
                webClient.Headers.Add(this.Headers);
            }

            if (this.Proxy != null)
            {
                webClient.Proxy = this.Proxy;
            }

            return webClient;
        }
    }
}
