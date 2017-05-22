namespace PH.Well.Api.Mapper
{
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Models;
    using WebGrease.Css.Extensions;

    public class WidgetWarningMapper : IWidgetWarningMapper
    {

       public WidgetWarning Map(WidgetWarningModel model)
        {
            var WidgetWarning = new WidgetWarning
            {
                Id = model.Id,
                WidgetName = model.WidgetName,
                WarningLevel = model.WarningLevel.Value,
                Type = (int)EnumExtensions.GetValueFromDescription<WidgetType>(model.Type)
            };

            model.Branches.ForEach(x => WidgetWarning.Branches.Add(x));

            return WidgetWarning;
        }


        public WidgetWarningModel Map(WidgetWarning widgetWarning)
        {
            var model = new WidgetWarningModel
            {
                Id = widgetWarning.Id,
                WidgetName = widgetWarning.WidgetName,
                WarningLevel = widgetWarning.WarningLevel,
                Type = EnumExtensions.GetDescription(widgetWarning.WidgetType)
                //ThresholdLevel = EnumExtensions.GetDescription(creditThreshold.ThresholdLevel),
            };

            foreach (var branch in widgetWarning.Branches)
            {
                model.BranchName += branch.Name + ", ";
                model.Branches.Add(branch);
            }

            model.BranchName = model.BranchName.TrimEnd(',', ' ');

            return model;
        }
    }
}