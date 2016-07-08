namespace PH.Well.TranSend.Contracts
{
    public interface IEpodSchemaProvider
    {
        bool IsFileValid(string sourceFile, string schemaFileout);
    }
}