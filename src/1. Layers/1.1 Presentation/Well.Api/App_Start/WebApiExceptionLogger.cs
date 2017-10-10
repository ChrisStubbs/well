//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace PH.Well.Api
//{
//    using System.Web.Http.ExceptionHandling;
//    using Common;
//    using Common.Contracts;

//    public class WebApiExceptionLogger : ExceptionLogger
//    {
//        private readonly ILogger logger;
//        private readonly IEventLogger eventLogger;

//        public WebApiExceptionLogger(ILogger logger, IEventLogger eventLogger)
//        {
//            this.logger = logger;
//            this.eventLogger = eventLogger;
//        }

//        public override void Log(ExceptionLoggerContext context)
//        {
//            logger.LogError(context.ExceptionContext.Exception.ToString());
//            eventLogger.TryWriteToEventLog(EventSource.WellApi, context.ExceptionContext.Exception);
//        }
//    }
//}