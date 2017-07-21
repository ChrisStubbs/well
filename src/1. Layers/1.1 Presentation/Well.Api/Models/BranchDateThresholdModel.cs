using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PH.Well.Api.Models
{
    public class BranchDateThresholdModel
    {
        public int BranchId { get; set; }
        public byte NumberOfDays { get; set; }
    }
}