namespace PH.Well.Api.Mapper.Contracts
{
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public interface ICleanPreferenceMapper
    {
        CleanPreferenceModel Map(CleanPreference cleanPreference);

        CleanPreference Map(CleanPreferenceModel model);
    }
}