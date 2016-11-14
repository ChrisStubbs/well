namespace PH.Well.Services.EpodImport
{
    using System;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;

    using Well.Services.Contracts;

    public class EpodSchemaValidator : IEpodSchemaValidator
    {
        private readonly ILogger logger;

        private readonly IEventLogger eventLogger;

        private bool validationOk = true;

        private string filePath;

        public EpodSchemaValidator(ILogger logger, IEventLogger eventLogger)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
        }

        public bool IsFileValid(string sourceFile, string schemaFile)
        {
            this.filePath = sourceFile;

            try
            {
                var xdoc = XDocument.Load(this.filePath);

                var schema = new XmlSchemaSet();

                schema.Add(string.Empty, schemaFile);

                xdoc.Validate(schema, ValidationCallback);
            }
            catch (Exception exception)
            {
                this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"{this.filePath} not loaded!", 3421);
                this.logger.LogError("Error occured when trying to load xml file!", exception);
                return false;
            }

            if (!this.validationOk) this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"{this.filePath} not loaded! Check the Nlog files", 3421);

            return this.validationOk;
        }

        private void ValidationCallback(object sender, ValidationEventArgs e)
        {
            this.logger.LogError($"{this.filePath} - Xml failed schema validation", e.Exception);
            this.validationOk = false;
        }
    }
}
