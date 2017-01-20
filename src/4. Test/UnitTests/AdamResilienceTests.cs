namespace PH.Well.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AIA.ADAM.DataProvider;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class AdamResilienceTests
    {
        [Test]
        public void FooMe()
        {
            // we have a credit to do in ADAM

            // we want to track where we are incase ADAM dies in the middle of the unit of work

            var header = "insert foo into header";

            var line1 = "insert bar in to line";

            var line2 = "insert bar2 in to line";

            var creditTransaction = new CreditEventTransaction {HeaderSql = header};

            creditTransaction.LineSql.Add("reason1", line1);
            creditTransaction.LineSql.Add("reason2", line2);

            // hows to serlise the object to json to store in the database
            // var creditTransactionJson = JsonConvert.SerializeObject(creditTransaction);
            // how to desierlise the json into the object
            // var creditTransactionDesierlised = JsonConvert.DeserializeObject<CreditEventTransaction>(creditTransactionJson);

            // step 1 before we enter the adam connection we build up our object
            using (var connection = new AdamConnection(GetConnection()))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        foreach (var lineGroup in creditTransaction.LineSql.GroupBy(x => x.Key))
                        {
                            foreach (var line in lineGroup)
                            {
                                command.CommandText = line.Value;
                                command.ExecuteNonQuery();
                                creditTransaction.LinesToRemove.Add(line.Key, line.Value);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // log me
                }
            }

            foreach (var linesToRemove in creditTransaction.LinesToRemove)
            {
                creditTransaction.LineSql.Remove(linesToRemove.Key);
            }

            if (creditTransaction.CanWriteHeader)
            {
                // write the header
            }
            else
            {
                // write the object to the event table for the events task too pick up later
            }

        }

        private static string GetConnection()
        {
            var connection = new AdamConnectionStringBuilder
            {
                Pooling = true,
                MinPoolSize = 1,
                MaxPoolSize = 32,
                ConnectTimeout = 60,
                TransactionMode = AdamTransaction.TransactionMode.Ignore,
                DataSource = "foo",
                Database = "foo",
                Port = 1,
                UID = "foo",
                PWD = "foo",
                OpenMode = AdamOpenMode.NonexclusiveReadWrite
            };

            return connection.ConnectionString;
        }
    }

    public class CreditEventTransaction
    {
        public CreditEventTransaction()
        {
            this.LineSql = new Dictionary<string, string>();
            this.LinesToRemove = new Dictionary<string, string>();
        }

        public string HeaderSql { get; set; }

        public Dictionary<string, string> LineSql { get; set; }

        public Dictionary<string, string> LinesToRemove { get; set; }

        public bool CanWriteHeader => !this.LineSql.Any();
    }
}
