namespace PH.Well.TranSend.Contracts
{
    using Enums;

    public interface IEpodDomainImportProvider
    {
        void ImportRouteHeader(string filename, EpodFileType fileType);
    }
}