namespace PH.Well.Services.EpodImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Schema;

    using Well.Services.Contracts;

    public class EpodSchemaProvider : IEpodSchemaProvider
    {
        public bool IsFileValid(string sourceFile, string schemaFile, List<string> schemaErrors)
        {
            var validationErrors = default(IList<Tuple<object, XmlSchemaException>>);

            try
            {
                var isValid = LbF.XmlSerialisation.XmlSerialisationHelper.IsValidXml(sourceFile, schemaFile, out validationErrors);

                if (!isValid)
                {
                    throw new XmlSchemaException($"{sourceFile} did not pass validation against schema file");
                }
                
            }
            catch 
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
