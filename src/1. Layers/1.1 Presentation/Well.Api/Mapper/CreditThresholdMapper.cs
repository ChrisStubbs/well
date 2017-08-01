namespace PH.Well.Api.Mapper
{
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.Extensions;

    using WebGrease.Css.Extensions;

    public class CreditThresholdMapper : ICreditThresholdMapper
    {
        public CreditThreshold Map(CreditThresholdModel model)
        {
            return new CreditThreshold
            {
                Id = model.Id,
                Threshold = model.Threshold.GetValueOrDefault(),
                Level = model.ThresholdLevel
            };
        }

        public CreditThresholdModel Map(CreditThreshold creditThreshold)
        {
            return new CreditThresholdModel
            {
                Id = creditThreshold.Id,
                ThresholdLevel = creditThreshold.Level,
                Threshold = creditThreshold.Threshold,
                Description = creditThreshold.Description
            };
        }
    }
}