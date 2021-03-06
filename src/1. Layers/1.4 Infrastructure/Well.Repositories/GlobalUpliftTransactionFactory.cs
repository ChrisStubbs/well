﻿using System;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Repositories
{
    public class GlobalUpliftTransactionFactory : IGlobalUpliftTransactionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public string LineSql(GlobalUpliftTransaction transaction)
        {
            if (!transaction.WriteLine)
            {
                throw new InvalidOperationException(
                    $"Transaction property {nameof(transaction.WriteLine)} is {transaction.WriteLine}");
            }

            var sql =
                $@"INSERT INTO WELLLINE.WELLINEREC (WELLINEGUID, WELLINERCDTYPE, WELLINESEQNUM, WELLINEPROD, WELLINEQTY, WELLINECRDREASON, WELLINEENDLINE)
                   VALUES ({transaction.Id},'{GlobalUpliftTransaction.WELLHDRCDTYPE}',1,{transaction.ProductCode},{transaction.Quantity},{GlobalUpliftTransaction.WELLINECRDREASON},1);";

            return sql;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        public string HeaderSql(GlobalUpliftTransaction transaction)
        {
            if (!transaction.WriteHeader)
            {
                throw new InvalidOperationException(
                    $"Transaction property {nameof(transaction.WriteHeader)} is {transaction.WriteHeader}");
            }

            var sql =
                $@"INSERT INTO WELLHEAD (WELLHDGUID, WELLHDRCDTYPE, WELLHDBRANCH, WELLHDACNO, WELLHDFLAG, WELLHDNEWDELDATE, WELLHDREVDELDATE, WELLHDLINECOUNT, WELLHDINVNO, WELLHDTEXT1 )
                  VALUES ({transaction.Id},'{GlobalUpliftTransaction.WELLHDRCDTYPE}',{transaction.BranchId},{
                        GetAccountNumber(transaction.AccountNumber)
                    },0,'{transaction.StartDate.ToShortDateString()}','{transaction.EndDate.ToShortDateString()}',1, {transaction.CsfNumber}, '{transaction.CustomerReference}');";

            return sql;
        }

        private int GetAccountNumber(string accountNumberString)
        {
            return (int)(Convert.ToDecimal(accountNumberString) * 1000);
        }

    }
}
