namespace PH.Well.Common
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using Contracts;

    public class ServerErrorResponseHandler : IServerErrorResponseHandler
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogWriter;

        public ServerErrorResponseHandler(IEventLogger eventLogWriter, ILogger logger)
        {
            this.eventLogWriter = eventLogWriter;
            this.logger = logger;
        }

        public HttpResponseMessage HandleException(HttpRequestMessage httpRequestMessage, Exception exception, string loggerInformation)
        {
            this.logger.LogError(loggerInformation, exception);
            this.eventLogWriter.TryWriteToEventLog(EventSource.WellApi, exception);

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
