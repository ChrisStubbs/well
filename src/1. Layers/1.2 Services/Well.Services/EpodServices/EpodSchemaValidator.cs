namespace PH.Well.Services.EpodImport
{
    using System;
    using System.Xml.Linq;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;

    using Well.Services.Contracts;

    public class EpodSchemaValidator : IEpodSchemaValidator
    {
        private readonly ILogger logger;

        private readonly IEventLogger eventLogger;

        private bool validationOk = true;

        private string filePath;

        public EpodSchemaValidator(ILogger logger, IEventLogger eventLogger, IFileTypeService fileTypeService)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
        }

        public bool IsFileValid(string sourceFile)
        {
            this.filePath = sourceFile;

            try
            {
                var xdoc = XDocument.Load(this.filePath);

                return true;
            }
            catch (Exception exception)
            {
                this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"{this.filePath} not loaded!", 3421);
                this.logger.LogError("Error occured when trying to load xml file!", exception);
                return false;
            }
        }
    }
}
