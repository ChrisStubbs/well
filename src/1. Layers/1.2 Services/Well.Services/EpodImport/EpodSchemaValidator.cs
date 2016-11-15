namespace PH.Well.Services.EpodImport
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.Extensions;

    using Well.Services.Contracts;

    public class EpodSchemaValidator : IEpodSchemaValidator
    {
        private readonly ILogger logger;

        private readonly IEventLogger eventLogger;

        private readonly IFileTypeService fileTypeService;

        private bool validationOk = true;

        private string filePath;

        public EpodSchemaValidator(ILogger logger, IEventLogger eventLogger, IFileTypeService fileTypeService)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.fileTypeService = fileTypeService;
        }

        public bool IsFileValid(string sourceFile)
        {
            var schemaFile = this.GetSchema(sourceFile);

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

            if (!this.validationOk)
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport, 
                    $"{this.filePath} not loaded! Check the Nlog files", 3421);

            return this.validationOk;
        }

        private void ValidationCallback(object sender, ValidationEventArgs e)
        {
            this.logger.LogError($"{this.filePath} - Xml failed schema validation", e.Exception);
            this.validationOk = false;
        }

        private string GetSchema(string filename)
        {
            var fileType = this.fileTypeService.DetermineFileType(filename.GetFilename());

            var schemaType = TransendSchemaType.Unknown;

            switch (fileType)
            {
                case EpodFileType.RouteHeader:
                    schemaType = TransendSchemaType.RouteHeaderSchema;
                    break;
                case EpodFileType.RouteEpod:
                    schemaType = TransendSchemaType.RouteEpodSchema;
                    break;
                case EpodFileType.OrderUpdate:
                    schemaType = TransendSchemaType.RouteUpdateSchema;
                    break;
                case EpodFileType.Unknown:
                    this.logger.LogDebug($"Unknown file indicator {filename}!");
                    break;
            }

            var schemaName = EnumExtensions.GetDescription(schemaType);

            return Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Schemas", schemaName);
        }
    }
}
