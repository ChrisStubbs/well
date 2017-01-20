﻿namespace PH.Well.Repositories
{
    using System;
    using System.Linq;
    using AIA.Adam.RFS;
    using AIA.ADAM.DataProvider;
    using Common;
    using Newtonsoft.Json;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class AdamRepository : IAdamRepository
    {
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IEventLogger eventLogger;

        public AdamRepository(ILogger logger, IJobRepository jobRepository, IExceptionEventRepository exceptionEventRepository, IEventLogger eventLogger)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.exceptionEventRepository = exceptionEventRepository;
            this.eventLogger = eventLogger;
        }

        public AdamResponse Credit(CreditEventTransaction creditTransaction, AdamSettings adamSettings, string username)
        {
            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        foreach (var line in creditTransaction.LineSql.OrderBy(x => x.Key))
                        {
                                command.CommandText = line.Value;
                                command.ExecuteNonQuery();
                                creditTransaction.LinesToRemove.Add(line.Key, line.Value);
                        }
                    }
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred writing credit line!", adamException);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                        $"Adam exception {adamException} when writing credit line for credit event transaction {creditTransaction.HeaderSql}",
                        2010);
                }
              }

            foreach (var linesToRemove in creditTransaction.LinesToRemove)
            {
                creditTransaction.LineSql.Remove(linesToRemove.Key);
            }

            if (creditTransaction.CanWriteHeader)
            {
                using (var connection = new AdamConnection(GetConnection(adamSettings)))
                {
                    try
                    {
                        connection.Open();

                        using (var command = new AdamCommand(connection))
                        {
                            command.CommandText = creditTransaction.HeaderSql;
                            command.ExecuteNonQuery();

                            return AdamResponse.Success;
                        }
                    }
                    catch (AdamProviderException adamException)
                    {
                        this.logger.LogError("ADAM error occurred writing credit header!", adamException);

                        this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                       $"Adam exception {adamException} when writing credit header for credit event transaction {creditTransaction.HeaderSql}",
                       2020);
                    }
                }
            }
            else
            {
                this.logger.LogError("ADAM error occurred writing credit line! Remaining credit details recorded.");
                // PART PROCESS
                this.exceptionEventRepository.CurrentUser = username;
                this.exceptionEventRepository.InsertCreditEventTransaction(creditTransaction);
                return AdamResponse.PartProcessed;
            }

            return AdamResponse.Unknown;
        }

        public AdamResponse ExecuteCredit(AdamSettings adamSettings, string commandString)
        {
            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        command.CommandText = commandString;
                        command.ExecuteNonQuery();
                    }


                    return AdamResponse.Success;
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred!", adamException);

                    if (adamException.AdamErrorId == AdamError.ADAMNOTRUNNING)
                    {
                        return AdamResponse.AdamDown;
                    }
                }
            }

            return AdamResponse.Unknown;
        }

        public AdamResponse CreditReorder(CreditReorderEvent creditReorder, AdamSettings adamSettings)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse Reject(RejectEvent reject, AdamSettings adamSettings)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse ReplanRoadnet(RoadnetEvent roadnet, AdamSettings adamSettings)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse ReplanTranscend(TranscendEvent transcend, AdamSettings adamSettings)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse ReplanQueue(QueueEvent queue, AdamSettings adamSettings)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse Grn(GrnEvent grn, AdamSettings adamSettings)
        {
            var job = this.jobRepository.GetById(grn.Id);
            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        var acno = (int)(Convert.ToDecimal(job.PhAccount) * 1000);
                        var today = DateTime.Now.ToShortDateString();
                        var now = DateTime.Now.ToShortTimeString();

                        var commandString =
                            string.Format(
                                "INSERT INTO WELLHEAD (WELLHDGUID, WELLHDCREDAT, WELLHDCRETIM, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDGRNCODE, WELLHDGRNRCPTREF) " +
                                "VALUES({0}, '{1}', '{2}', {3}, '{4}', {5}, {6}, {7}, {8}, {9});", grn.Id, today, now, (int)EventAction.Grn , "WELL", grn.BranchId, acno, job.InvoiceNumber, 1, job.GrnNumberUpdate);

                        command.CommandText = commandString;
                        command.ExecuteNonQuery();

                    }
                    return AdamResponse.Success;
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred!", adamException);

                    if (adamException.AdamErrorId == AdamError.ADAMNOTRUNNING)
                    {
                        return AdamResponse.AdamDown;
                    }
                }
            }

            return AdamResponse.Unknown;
        }

        private static string GetConnection(AdamSettings settings)
        {
            var connection = new AdamConnectionStringBuilder
            {
                Pooling = true,
                MinPoolSize = 1,
                MaxPoolSize = 32,
                ConnectTimeout = 60,
                TransactionMode = AdamTransaction.TransactionMode.Ignore,
                DataSource = settings.Server,
                Database = settings.Rfs,
                Port = settings.Port,
                UID = settings.Username,
                PWD = settings.Password,
                OpenMode = AdamOpenMode.NonexclusiveReadWrite
            };

            return connection.ConnectionString;
        }
    }
}