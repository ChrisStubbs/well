namespace PH.Well.Api.Mapper.Contracts
{
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public interface ISeasonalDateMapper
    {
        SeasonalDate Map(SeasonalDateModel model);

        SeasonalDateModel Map(SeasonalDate seasonalDate);
    }
}