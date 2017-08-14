using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts.Data
{
    /// <summary>
    /// Defines single uplift import record
    /// </summary>
    public interface IUpliftData
    {
        int Id { get; }

        int BranchId { get; }

        string AccountNumber { get; }

        string CreditReasonCode { get; }

        int ProductCode { get; }

        int Quantity { get; }

        DateTime StartDate { get; }

        DateTime EndDate { get; }

        string CustomerReference { get; }
    }
}
