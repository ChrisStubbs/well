using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.ValueObjects
{
    public class ExceptionTotalsPerRoute
    {
        public int? BranchId { get; set; }

        public int Routeid { get; set; }

        public int? WithExceptions { get; set; }

        public int? WithOutExceptions { get; set; }
    }
}
