using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.ValueObjects
{
    using Common.Extensions;
    using Enums;

    public class GlobalUpliftTransaction
    {
        /// <summary>
        /// ADAM process type
        /// </summary>
        public const int WELLHDRCDTYPE = (int)EventAction.GlobalUplift;

    /// <summary>
    /// ADAM credit reason code
    /// </summary>
    public const int WELLINECRDREASON = 24;

        /// <summary>
        /// This constructor allows to create transaction and specify which lines should be written
        /// </summary>
        /// <param name="id"></param>
        /// <param name="branchId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="creditReason"></param>
        /// <param name="productCode"></param>
        /// <param name="quantity"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="writeLine"></param>
        /// <param name="writeHeader"></param>
        /// <param name="csfNumber"></param>
        /// <param name="customerReference"></param>
        public GlobalUpliftTransaction(int id, int branchId, string accountNumber, string creditReason, int productCode,
            int quantity, DateTime startDate, DateTime endDate, bool writeLine, bool writeHeader, int csfNumber, string customerReference)
        {
            Id = id;
            BranchId = branchId;
            AccountNumber = accountNumber;
            CreditReasonCode = creditReason;
            ProductCode = productCode;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            WriteLine = writeLine;
            WriteHeader = writeHeader;
            CsfNumber = csfNumber;
            CustomerReference = customerReference;
        }

        /// <summary>
        /// This constructor created new transaction with default lines to write
        /// </summary>
        /// <param name="id"></param>
        /// <param name="branchId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="creditReason"></param>
        /// <param name="productCode"></param>
        /// <param name="quantity"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="csfNumber"></param>
        /// <param name="customerReference"></param>
        public GlobalUpliftTransaction(int id, int branchId, string accountNumber, string creditReason, int productCode,
            int quantity, DateTime startDate, DateTime endDate, int csfNumber, string customerReference) : this(id, branchId, accountNumber, creditReason,
            productCode,
            quantity, startDate, endDate, true, true, csfNumber, customerReference)
        {
            Id = id;
            BranchId = branchId;
            AccountNumber = accountNumber;
            CreditReasonCode = creditReason;
            ProductCode = productCode;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            CsfNumber = csfNumber;
            CustomerReference = customerReference;
        }

        public int Id { get; }

        public int BranchId { get; }

        public string AccountNumber { get; }

        public string CreditReasonCode { get; }

        public int ProductCode { get; }

        public int Quantity { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public int CsfNumber { get; }

        public string CustomerReference { get; }

        /// <summary>
        /// Should write line
        /// </summary>
        public bool WriteLine { get; }

        /// <summary>
        /// Has line been written
        /// </summary>
        public bool LineDidWrite { get; set; }

        /// <summary>
        /// Should write header
        /// </summary>
        public bool WriteHeader { get; }

        /// <summary>
        /// Has header been
        /// </summary>
        public bool HeaderDidWrite { get; set; }
    }
}
