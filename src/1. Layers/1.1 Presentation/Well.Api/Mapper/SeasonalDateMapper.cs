namespace PH.Well.Api.Mapper
{
    using System;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    using WebGrease.Css.Extensions;

    public class SeasonalDateMapper : ISeasonalDateMapper
    {
        public SeasonalDate Map(SeasonalDateModel model)
        {
            var seasonalDate = new SeasonalDate
            {
                Id = model.Id,
                Description = model.Description,
                From = model.FromDate,
                To = model.ToDate
            };

            model.Branches.ForEach(x => seasonalDate.Branches.Add(x));

            return seasonalDate;
        }

        public SeasonalDateModel Map(SeasonalDate seasonalDate)
        {
            var model = new SeasonalDateModel
            {
                Id = seasonalDate.Id,
                Description = seasonalDate.Description,
                FromDate = seasonalDate.From,
                ToDate = seasonalDate.To
            };

            foreach (var branch in seasonalDate.Branches)
            {
                model.BranchName += branch.Name + ", ";
                model.Branches.Add(branch);
            }

            model.BranchName = model.BranchName.TrimEnd(',', ' ');

            return model;
        }
    }
}