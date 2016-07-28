namespace PH.Well.Services.EpodImport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public  class XmlSerialisationHelper
    {

        public bool IsValidXml(string xmlFileUrl, string xmlSchemaFile)
        {
            IList<Tuple<object, XmlSchemaException>> _;

            return IsValidXml(xmlFileUrl, xmlSchemaFile, out _);
        }

        public  bool IsValidXml(string xmlFileUrl, string xmlSchemaFile, out IList<Tuple<object, XmlSchemaException>> validationErrors)
        {
            var internalValidationErrors = new List<Tuple<object, XmlSchemaException>>();
            var readerSettings = XmlSchemaReader(xmlSchemaFile,
                (obj, eventArgs) => internalValidationErrors.Add(new Tuple<object, XmlSchemaException>(obj, eventArgs.Exception))
            );

            using (var xmlReader = new XmlTextReader(xmlFileUrl))
            using (var objXmlReader = XmlReader.Create(xmlReader, readerSettings))
            {
                try
                {
                    while (objXmlReader.Read()) { }
                }
                catch (XmlSchemaException exception)
                {
                    internalValidationErrors.Add(new Tuple<object, XmlSchemaException>(objXmlReader, exception));
                }
            }

            validationErrors = internalValidationErrors;

            return !validationErrors.Any();
        }

        private XmlReaderSettings XmlSchemaReader(string xmlSchemaFile, Action<object, ValidationEventArgs> validationFunction)
        {
            var xsdReader = new StreamReader(xmlSchemaFile);
            var schema = XmlSchema.Read(xsdReader, (obj, eventArgs) => validationFunction(obj, eventArgs));

            var readerSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            readerSettings.Schemas.Add(schema);

            return readerSettings;
        }
    }

    public class XmlSerialisationHelper<T>
    {
        private Type SerialiseType { get; set; }


        public XmlSerialisationHelper()
        {
            SerialiseType = typeof(T);
        }

        public void Serialise(string xmlFileUrl, T obj)
        {
            using (TextWriter textWriter = new StreamWriter(xmlFileUrl))
            {
                var serializer = new XmlSerializer(SerialiseType);
                serializer.Serialize(textWriter, obj);
            }
        }

        public T Deserialise(string xmlFileUrl)
        {
            var xmlRootAttribute = new XmlRootAttribute();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFileUrl);

                if (xmlDoc.DocumentElement == null)
                    return default(T);

                xmlRootAttribute.ElementName = xmlDoc.DocumentElement.Name;
            }
            catch (IOException)
            {
                return default(T);
            }

            using (TextReader textReader = new StreamReader(xmlFileUrl))
            {
                var deserializer = new XmlSerializer(SerialiseType, xmlRootAttribute);
                return (T)deserializer.Deserialize(textReader);
            }
        }

        public T Deserialise(string xmlFileUrl, XmlRootAttribute xmlRootAttribute)
        {
            using (TextReader textReader = new StreamReader(xmlFileUrl))
            {
                var deserializer = new XmlSerializer(SerialiseType, xmlRootAttribute);
                return (T)deserializer.Deserialize(textReader);
            }
        }
    }
}
