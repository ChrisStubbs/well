using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.ValueObjects
{
    public class GlobalUpliftEvent
    {
        public int Id { get; set; }

        public int BranchId { get; set; }

        public string AccountNumber { get; set; }

        public string CreditReasonCode { get; set; }

        public int ProductCode { get; set; }

        public int Quantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// Has line been written
        /// </summary>
        public bool WriteLine { get; set; }

        /// <summary>
        /// Has header been written
        /// </summary>
        public bool WriteHeader { get; set; }
    }
}
