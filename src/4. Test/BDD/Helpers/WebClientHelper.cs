namespace PH.Well.BDD.Helpers
{
   
        using System;
        using System.IO;
        using System.Net;
        using Common.Contracts;
        using StructureMap;

        public class WebClientHelper : IWebClientHelper
        {
            private readonly IWebClient webClient;

            public WebClientHelper(IWebClient webClient)
            {
                this.webClient = webClient;
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            }

            public string Get(string url)
            {
                try
                {
                    Response = this.webClient.DownloadString(url);
                    HttpWebResponse = webClient.HttpWebResponse;
                    return Response;
                }
                catch (Exception ex)
                {
                    var webEx = ex as WebException;
                    if (webEx != null)
                    {
                        HttpWebResponse = (HttpWebResponse)webEx.Response;
                        using (var s = new StreamReader(webEx.Response.GetResponseStream()))
                        {
                            Response = s.ReadToEnd();
                        }
                        return Response;
                    }
                    throw;
                }

            }

            public string Post(string url, string data)
            {
                return UploadStringAndThrow(url, "POST", data);
            }


            public string UploadString(string url, string method, string data)
            {
                try
                {
                    Response = webClient.UploadString(url, method, data);
                    HttpWebResponse = webClient.HttpWebResponse;
                    return Response;

                }
                catch (Exception ex)
                {
                    var webEx = ex as WebException;
                    if (webEx != null)
                    {
                        HttpWebResponse = (HttpWebResponse)webEx.Response;

                        using (var s = new StreamReader(webEx.Response.GetResponseStream()))
                        {
                            Response = s.ReadToEnd();
                            return Response;
                        }
                    }
                    throw;
                }
            }

            public string Put(string url, string data)
            {
                return UploadStringAndThrow(url, "PUT", data);
            }

            public string Patch(string url, string data)
            {
                return UploadString(url, "PATCH", data);
            }

            public string UploadStringAndThrow(string url, string method, string data)
            {
                try
                {
                    Response = webClient.UploadString(url, method, data);
                    HttpWebResponse = webClient.HttpWebResponse;
                    return Response;
                }
                catch (Exception ex)
                {
                    var webEx = ex as WebException;
                    if (webEx != null)
                    {
                        HttpWebResponse = (HttpWebResponse)webEx.Response;

                        using (var s = new StreamReader(webEx.Response.GetResponseStream()))
                        {
                            Response = s.ReadToEnd();
                            throw new Exception($"Error when {method} to URL {url} Response: {Response}", webEx);
                        }

                    }
                    throw;
                }
            }


            public HttpWebResponse HttpWebResponse { get; private set; }

            public string Response { get; set; }


        }
    

}
