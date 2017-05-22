namespace PH.Well.Api.Mapper.Contracts
{
    using Domain;
    using Models;

    public interface IWidgetWarningMapper
    {
        WidgetWarning Map(WidgetWarningModel model);

        WidgetWarningModel Map(WidgetWarning widgetWarning);
         
    }
}