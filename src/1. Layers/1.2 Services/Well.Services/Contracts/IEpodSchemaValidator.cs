namespace PH.Well.Services.Contracts
{
    public interface IEpodSchemaValidator
    {
        bool IsFileValid(string sourceFile, string schemaFile);
    }
}