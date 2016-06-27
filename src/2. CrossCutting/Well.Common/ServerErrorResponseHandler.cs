namespace PH.Well.Common
{
    using System;
    using System.Diagnostics.Tracing;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Text;
    using Contracts;

    public class ServerErrorResponseHandler : IServerErrorResponseHandler
    {
        private readonly IEventLogger eventLogWriter;

        public ServerErrorResponseHandler(IEventLogger eventLogWriter)
        {
            this.eventLogWriter = eventLogWriter;
        }

        public HttpResponseMessage HandleException(HttpRequestMessage httpRequestMessage, Exception exception)
        {
            eventLogWriter.TryWriteToEventLog(EventSource.WellApi, exception);

            var catchAllErrorResponse = httpRequestMessage.CreateErrorResponse(HttpStatusCode.InternalServerError, this.GetMessage(exception));

            return catchAllErrorResponse;
        }

        private string GetMessage(Exception exception)
        {
            var errorMessage = new StringBuilder();
#if DEBUG
            errorMessage.AppendLine(exception.Message);

            if (exception.InnerException != null)
            {
                errorMessage.AppendLine(exception.InnerException.Message);
            }
#else
            errorMessage.Append("An error occured. The system administrator has been notified.");
#endif
            return errorMessage.ToString();
        }
    }
}
