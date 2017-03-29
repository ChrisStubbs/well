using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PH.Well.Api.Models
{
    using Domain.Enums;

    public class BulkCreditModel
    {
        public IList<int> JobIds { get; set; }
        public JobDetailReason Reason { get; set; }
        public JobDetailSource Source { get; set; }
    }
}