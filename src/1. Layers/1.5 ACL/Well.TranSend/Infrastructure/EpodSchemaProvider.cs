namespace PH.Well.TranSend.Infrastructure
{
    using System.Xml;
    using System.Xml.Schema;
    using Common.Contracts;
    using Contracts;

    public class EpodSchemaProvider : IEpodSchemaProvider
    {
        private XmlSchemaSet schemas;
        private bool validFile = true;
        private readonly ILogger logger;


        public EpodSchemaProvider(ILogger logger)
        {
            this.logger = logger;
        }

        public bool IsFileValid(string sourceFile, string schemaFile)
        {
            
            schemas = new XmlSchemaSet();
            schemas.Add("", schemaFile);

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemas
            };

            settings.ValidationEventHandler += ValidationError;

            var reader = XmlReader.Create(sourceFile, settings);
            while (reader.Read())
            {
            }

            reader.Close();
            return validFile;
        }

        private void ValidationError(object sender,
         ValidationEventArgs arguments)
        {
            logger.LogError(arguments.Message); 
            validFile = false; 
        }

     
    }
}
