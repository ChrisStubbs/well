namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain.Enums;

    public interface IFileTypeService
    {
        EpodFileType DetermineFileType(string filename);
    }
}