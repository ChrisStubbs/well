namespace PH.Well.Common.Contracts
{
    using System;
    using System.Net.Http;

    public interface IServerErrorResponseHandler
    {
        HttpResponseMessage HandleException(HttpRequestMessage httpRequestMessage, Exception exception, string loggerInformation);
    }
}
