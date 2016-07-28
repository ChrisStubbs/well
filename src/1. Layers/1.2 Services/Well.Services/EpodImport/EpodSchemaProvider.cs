namespace PH.Well.Services.EpodImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;
    using Common.Contracts;
    using Well.Services.Contracts;
    using LbF.XmlSerialisation;

    public class EpodSchemaProvider : IEpodSchemaProvider
    {
        private XmlSchemaSet schemas;
        private bool validFile = true;
        private readonly ILogger logger;
      


        public EpodSchemaProvider(ILogger logger)
        {
            this.logger = logger;
        }

        public bool IsFileValid(string sourceFile, string schemaFile, ref List<string> schemaErrors)
        {

            var validationErrors = default(IList<Tuple<object, XmlSchemaException>>);
            schemaErrors = new List<string>();

            try
            {

                var isValid = LbF.XmlSerialisation.XmlSerialisationHelper.IsValidXml(sourceFile, schemaFile, out validationErrors);

                if(!isValid)
                    throw new XmlSchemaException($"{sourceFile} did not pass validation against schema file");
            }
            catch (Exception)
            {
                if (validationErrors != null && validationErrors.Any())
                {
                    foreach (var error in validationErrors)
                    {
                        schemaErrors.Add($"{error.Item1}:\t{error.Item2.Message}");
                    }
                }

                return false;
            }

            return true;
        }

     
    }
}
