using PH.Well.Domain.Enums;

namespace PH.Well.Api.Models
{
    public class CreditThresholdModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public ThresholdLevel ThresholdLevel { get; set; }

        public decimal? Threshold { get; set; }
    }
}