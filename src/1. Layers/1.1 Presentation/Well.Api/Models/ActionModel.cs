namespace PH.Well.Api.Models
{
    using System;
    using Domain.Enums;
    using Domain.Extensions;

    public class ActionModel
    {
        public int Quantity { get; set; }

        public ExceptionAction Action { get; set; }
        public string ActionDescription => EnumExtensions.GetDescription(Action);

        public ActionStatus Status { get; set; }
        public string StatusDescription => Status.ToString();
    }
}