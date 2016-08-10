namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Common.Contracts;

    public interface IEpodSchemaProvider
    {
        bool IsFileValid(string sourceFile, string schemaFile, List<string> schemaErrors, ILogger logger);
    }
}