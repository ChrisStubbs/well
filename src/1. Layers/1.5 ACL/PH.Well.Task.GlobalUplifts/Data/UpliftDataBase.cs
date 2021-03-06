﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts.Data
{
    /// <summary>
    /// Base class for uplift data
    /// </summary>
    public class UpliftDataBase : IUpliftData
    {
        #region Private fields
        public int Id { get; }
        public int BranchId { get; }
        public string AccountNumber { get; }
        public string CreditReasonCode { get; }
        public int ProductCode { get; }
        public int Quantity { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string CustomerReference { get; }
        #endregion Private fields

        #region Constructors
        public UpliftDataBase(int id,int branchId, string accountNumber, string creditReasonCode, int productCode,
            int quantity, DateTime startDate, DateTime endDate, string customerReference)
        {
            Id = id;
            BranchId = branchId;
            AccountNumber = accountNumber;
            CreditReasonCode = creditReasonCode;
            ProductCode = productCode;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
            CustomerReference = customerReference;
        }
        #endregion Constructors
    }
}
