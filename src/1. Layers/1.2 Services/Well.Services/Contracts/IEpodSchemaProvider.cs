namespace PH.Well.Services.Contracts
{
    public interface IEpodSchemaProvider
    {
        bool IsFileValid(string sourceFile, string schemaFileout);
    }
}