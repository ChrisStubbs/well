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
            var creditThreshold = new CreditThreshold
            {
                Id = model.Id,
                Value = model.Threshold.Value,
                ThresholdLevelId = (int)EnumExtensions.GetValueFromDescription<ThresholdLevel>(model.ThresholdLevel)
            };

            model.Branches.ForEach(x => creditThreshold.Branches.Add(x));

            return creditThreshold;
        }

        public CreditThresholdModel Map(CreditThreshold creditThreshold)
        {
            var model = new CreditThresholdModel
            {
                Id = creditThreshold.Id,
                ThresholdLevel = EnumExtensions.GetDescription(creditThreshold.ThresholdLevel),
                Threshold = creditThreshold.Value
            };

            foreach (var branch in creditThreshold.Branches)
            {
                model.BranchName += branch.Name + ", ";
                model.Branches.Add(branch);
            }

            model.BranchName = model.BranchName.TrimEnd(',', ' ');

            return model;
        }
    }
}