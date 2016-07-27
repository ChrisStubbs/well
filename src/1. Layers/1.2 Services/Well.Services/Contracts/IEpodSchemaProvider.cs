namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Schema;

    public interface IEpodSchemaProvider
    {
        bool IsFileValid(string sourceFile, string schemaFile, out List<string> schemaErrors);
    }
}