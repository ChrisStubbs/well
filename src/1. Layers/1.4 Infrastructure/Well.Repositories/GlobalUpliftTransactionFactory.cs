using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Repositories
{
    public class GlobalUpliftTransactionFactory : IGlobalUpliftTransactionFactory
    {
        private const string WELLHDRCDTYPE = "10";

        public string LineSql(GlobalUpliftTransaction transaction)
        {
            var sql =
                $@"INSERT INTO WELLLINE (WELLINEGUID, WELLLINERCDTYPE, WELLINESEQNUM, WELLINEPROD, WELLINEQTY, WELLINECREDREASON, WELLINEENDLINE)
                   VALUES ({transaction.Id},'{WELLHDRCDTYPE}',1,{transaction.ProductCode},{transaction.Quantity},'{transaction.CreditReasonCode}',1);";

            return sql;
        }

        public string HeaderSql(GlobalUpliftTransaction transaction)
        {
            var sql =
                $@"INSERT INTO WELLHEAD (WELLHEADGUID, WELLHDRCDTYPE, WELLHDBRANCH, WELLHDACNO, WELLHDFLAG, WELLHDNEWDELDATE, WELLHDREVDELDATE, WELLHDLINECOUNT )
                  VALUES ({transaction.Id},'{WELLHDRCDTYPE}',{transaction.BranchId},{
                        GetAccountNumber(transaction.AccountNumber)
                    },0,'{transaction.StartDate.ToShortTimeString()}','{transaction.EndDate.ToShortTimeString()}',1);";

            return sql;
        }

        private int GetAccountNumber(string accountNumberString)
        {
            return (int)(Convert.ToDecimal(accountNumberString) * 1000);
        }
    }
}
