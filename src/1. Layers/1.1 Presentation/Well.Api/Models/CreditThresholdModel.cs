using PH.Well.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PH.Well.Api.Models
{
    public class CreditThresholdModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public ThresholdLevel ThresholdLevel { get; set; }

        [Required]
        [Range(1, 1000000)]
        public decimal? Threshold { get; set; }
    }
}