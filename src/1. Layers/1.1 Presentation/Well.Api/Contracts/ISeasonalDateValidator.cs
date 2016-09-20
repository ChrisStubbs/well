namespace PH.Well.Api.Contracts
{
    using System.Text;

    using PH.Well.Api.Models;

    public interface ISeasonalDateValidator
    {
        StringBuilder Validate(SeasonalDateModel model);
    }
}