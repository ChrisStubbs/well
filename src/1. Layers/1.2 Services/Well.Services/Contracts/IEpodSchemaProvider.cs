namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    public interface IEpodSchemaProvider
    {
        bool IsFileValid(string sourceFile, string schemaFile, List<string> schemaErrors);
    }
}