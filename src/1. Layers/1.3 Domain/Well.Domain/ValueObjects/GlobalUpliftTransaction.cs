using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.ValueObjects
{
    public class GlobalUpliftTransaction
    {
        /// <summary>
        /// ADAM process type
        /// </summary>
        public const int WELLHDRCDTYPE = 10;

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
        public GlobalUpliftTransaction(int id, int branchId, string accountNumber, string creditReason, int productCode,
            int quantity, DateTime startDate, DateTime endDate, bool writeLine, bool writeHeader)
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
        public GlobalUpliftTransaction(int id, int branchId, string accountNumber, string creditReason, int productCode,
            int quantity, DateTime startDate, DateTime endDate) : this(id, branchId, accountNumber, creditReason,
            productCode,
            quantity, startDate, endDate, true, true)
        {
            Id = id;
            BranchId = branchId;
            AccountNumber = accountNumber;
            CreditReasonCode = creditReason;
            ProductCode = productCode;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int Id { get; }

        public int BranchId { get; }

        public string AccountNumber { get; }

        public string CreditReasonCode { get; }

        public int ProductCode { get; }

        public int Quantity { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

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
