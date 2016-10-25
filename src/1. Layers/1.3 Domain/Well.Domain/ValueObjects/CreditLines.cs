using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.ValueObjects
{
    public class CreditLines
    {
        public int CreditId { get; set; }
        public bool IsPending { get; set; }
    }
}
