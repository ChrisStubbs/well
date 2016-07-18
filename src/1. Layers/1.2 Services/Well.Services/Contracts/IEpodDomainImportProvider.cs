namespace PH.Well.Services.Contracts
{
    using Domain.Enums;

    public interface IEpodDomainImportProvider
    {
        void ImportRouteHeader(string filename, EpodFileType fileType);
    }
}