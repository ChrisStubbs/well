using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace PH.Well.UnitTests
{
    [TestFixture]
    public class MSDTC
    {
        /// <summary>
        /// This method contains code that can test whether DTC can be used
        /// </summary>
        [Test]
        [Explicit("Not specific to application functionality")]
        public void TestDtcCapability()
        {
            string connectionString =
                @"Data Source=ho-ms-dbprd2\SQLSERVER2014;Failover Partner=;Initial Catalog=Well;Integrated Security=True";

            using (TransactionScope transactionScope = new TransactionScope())
            {
               
                SqlConnection connectionOne = new SqlConnection(connectionString);
                SqlConnection connectionTwo = new SqlConnection(connectionString);

                try
                {
                    //2 connections, nested
                    connectionOne.Open();
                    connectionTwo.Open(); // escalates to DTC on 05 and 08
                    connectionTwo.Close();
                    connectionOne.Close();
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    connectionOne.Dispose();
                    connectionTwo.Dispose();
                }
            }
        }
    }
}
