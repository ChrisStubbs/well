using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.ValueObjects
{
    public class GlobalUpliftTransaction
    {
        public GlobalUpliftTransaction(int branchId, string accountNumber, string creditReason, int productCode, int quantity, DateTime startDate, DateTime endDate)
        {
            BranchId = branchId;
            AccountNumber = accountNumber;
            CreditReasonCode = creditReason;
            ProductCode = productCode;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
        }

        int BranchId { get; }

        string AccountNumber { get; }

        string CreditReasonCode { get; }

        int ProductCode { get; }

        int Quantity { get; }

        DateTime StartDate { get; }

        DateTime EndDate { get; }
    }
}
