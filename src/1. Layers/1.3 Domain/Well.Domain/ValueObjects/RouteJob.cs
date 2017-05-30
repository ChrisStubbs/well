namespace PH.Well.Domain.ValueObjects
{
    using Enums;
    using Extensions;

    public class RouteJob
    {
        public int RouteId { get; set; }
        public int JobId { get; set; }
        public string JobTypeCode { get; set; }
        public JobType JobType => EnumExtensions.GetValueFromDescription<JobType>(JobTypeCode);
    }
}