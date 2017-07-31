﻿namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AIA.Adam.RFS;
    using AIA.ADAM.DataProvider;
    using Common;
    using Domain;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;


    public class AdamRepository : IAdamRepository
    {
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IEventLogger eventLogger;
        private readonly IPodTransactionFactory podTransactionFactory;
        private readonly IExceptionEventRepository eventRepository;
        private readonly IGlobalUpliftTransactionFactory _globalUpliftTransactionFactory;

        public AdamRepository(ILogger logger, IJobRepository jobRepository, IEventLogger eventLogger, IPodTransactionFactory podTransactionFactory, IDeliveryReadRepository deliveryReadRepository, IExceptionEventRepository eventRepository,IGlobalUpliftTransactionFactory globalUpliftTransactionFactory)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.deliveryReadRepository = deliveryReadRepository;
            this.eventLogger = eventLogger;
            this.podTransactionFactory = podTransactionFactory;
            this.eventRepository = eventRepository;
            _globalUpliftTransactionFactory = globalUpliftTransactionFactory;
        }

        public AdamResponse Credit(CreditTransaction creditTransaction, AdamSettings adamSettings)
        {
            var linesToRemove = new Dictionary<int, string>();

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
                                linesToRemove.Add(line.Key, line.Value);
                        }
                    }
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred writing credit line!", adamException);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                        $"Adam exception {adamException} when writing credit line for credit event transaction {creditTransaction.HeaderSql}",
                        EventId.AdamCreditException);
                }
              }

            foreach (var line in linesToRemove)
            {
                creditTransaction.LineSql.Remove(line.Key);
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
                       EventId.AdamCreditHeaderException);
                    }
                }
            }
            else
            {
                this.logger.LogError("ADAM error occurred writing credit line! Remaining credit details recorded.");
                return AdamResponse.AdamDown;
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
            var delivery = this.deliveryReadRepository.GetDeliveryById(grn.Id, this.jobRepository.CurrentUser);
            if (delivery.GrnNumber != String.Empty)
            {
                using (var connection = new AdamConnection(GetConnection(adamSettings)))
                {
                    try
                    {
                        connection.Open();

                        using (var command = new AdamCommand(connection))
                        {
                            var acno = (int) (Convert.ToDecimal(delivery.AccountCode)*1000);
                            var today = DateTime.Now.ToShortDateString();
                            var now = DateTime.Now.ToShortTimeString();

                            var grnNumeric = 0;
                            var result = Int32.TryParse(delivery.GrnNumber, out grnNumeric);
                            if (!result)
                            {
                                 grnNumeric = 0;
                            }

                            var commandString =
                                string.Format(
                                    "INSERT INTO WELLHEAD (WELLHDGUID, WELLHDCREDAT, WELLHDCRETIM, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDGRNCODE, WELLHDGRNRCPTREF) " +
                                    "VALUES({0}, '{1}', '{2}', {3}, '{4}', {5}, {6}, {7}, {8}, {9});", grn.Id, today,
                                    now, (int) EventAction.Grn, "WELL", grn.BranchId, acno, delivery.InvoiceNumber,
                                    delivery.GrnProcessType, grnNumeric);

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
            }

            return AdamResponse.Unknown;
        }

        public AdamResponse PodTransaction(PodTransaction pod, AdamSettings adamSettings)
        {
            var job = this.jobRepository.GetById(pod.JobId);
            var linesToRemove = new Dictionary<int, string>();

            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        foreach (var line in pod.LineSql.OrderBy(x => x.Key))
                        {
                            command.CommandText = line.Value;
                            command.ExecuteNonQuery();
                            linesToRemove.Add(line.Key, line.Value);
                        }
                    }
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred writing credit line!", adamException);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                        $"Adam exception {adamException} when writing pod credit line for pod transaction {pod.HeaderSql}",
                        EventId.AdamPodException);
                }
            }

            foreach (var line in linesToRemove)
            {
                pod.LineSql.Remove(line.Key);
            }

            if (pod.CanWriteHeader)
            {
                using (var connection = new AdamConnection(GetConnection(adamSettings)))
                {
                    try
                    {
                        connection.Open();

                        using (var command = new AdamCommand(connection))
                        {
                            command.CommandText = pod.HeaderSql;
                            command.ExecuteNonQuery();

                            return AdamResponse.Success;
                        }
                    }
                    catch (AdamProviderException adamException)
                    {
                        this.logger.LogError("ADAM error occurred writing pod header!", adamException);

                        this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                       $"Adam exception {adamException} when writing credit header for pod transaction {pod.HeaderSql}",
                       EventId.AdamPodHeaderException);
                    }
                }
            }
            else
            {
                this.logger.LogError("ADAM error occurred writing pod! Remaining pod details recorded.");
                return AdamResponse.AdamDown;
            }

            return AdamResponse.Unknown;
        }
        

        public AdamResponse Pod(PodEvent podEvent, AdamSettings adamSettings, Job job)
        {
            //var job = this.jobRepository.GetById(podEvent.Id);
            var pod = podTransactionFactory.Build(job, podEvent.BranchId);
            var linesToRemove = new Dictionary<int, string>();

            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        foreach (var line in pod.LineSql.OrderBy(x => x.Key))
                        {
                            command.CommandText = line.Value;
                            command.ExecuteNonQuery();
                            linesToRemove.Add(line.Key, line.Value);
                        }
                    }
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred writing credit line!", adamException);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                        $"Adam exception {adamException} when writing pod credit line for pod transaction {pod.HeaderSql}",
                        EventId.AdamPodCreditLineException);
                }
            }

            foreach (var line in linesToRemove)
            {
                pod.LineSql.Remove(line.Key);
            }

            if (pod.CanWriteHeader)
            {
                using (var connection = new AdamConnection(GetConnection(adamSettings)))
                {
                    try
                    {
                        connection.Open();

                        using (var command = new AdamCommand(connection))
                        {
                            command.CommandText = pod.HeaderSql;
                            command.ExecuteNonQuery();

                            return AdamResponse.Success;
                        }
                    }
                    catch (AdamProviderException adamException)
                    {
                        this.eventRepository.InsertPodTransaction(pod);
                        this.logger.LogError("ADAM error occurred writing pod header!", adamException);

                        this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                       $"Adam exception {adamException} when writing credit header for pod transaction {pod.HeaderSql}",
                       EventId.AdamPodCreditPodTransactionException);
                    }
                }
            }
            else
            {
                this.eventRepository.InsertPodTransaction(pod);
                this.logger.LogError("ADAM error occurred writing pod! Remaining pod details recorded.");
                return AdamResponse.AdamDown;
            }

            return AdamResponse.Unknown;
        }

        public AdamResponse AmendmentTransaction(AmendmentTransaction amend, AdamSettings adamSettings)
        {
            var linesToRemove = new Dictionary<int, string>();

            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        foreach (var line in amend.LineSql.OrderBy(x => x.Key))
                        {
                            command.CommandText = line.Value;
                            command.ExecuteNonQuery();
                            linesToRemove.Add(line.Key, line.Value);
                        }
                    }
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred writing amendment line!", adamException);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                        $"Adam exception {adamException} when writing amendment line for amend transaction {amend.HeaderSql}",
                        EventId.AdamAmendmentTransactionException);
                }
            }

            foreach (var line in linesToRemove)
            {
                amend.LineSql.Remove(line.Key);
            }

            if (amend.CanWriteHeader)
            {
                using (var connection = new AdamConnection(GetConnection(adamSettings)))
                {
                    try
                    {
                        connection.Open();

                        using (var command = new AdamCommand(connection))
                        {
                            command.CommandText = amend.HeaderSql;
                            command.ExecuteNonQuery();

                            return AdamResponse.Success;
                        }
                    }
                    catch (AdamProviderException adamException)
                    {
                        this.logger.LogError("ADAM error occurred writing amend header!", adamException);

                        this.eventLogger.TryWriteToEventLog(EventSource.WellApi,
                       $"Adam exception {adamException} when writing amend header for amend transaction {amend.HeaderSql}",
                       EventId.AdamAmendmentHeaderTransactionException);
                    }
                }
            }
            else
            {
                this.logger.LogError("ADAM error occurred writing amendment! Remaining amend details recorded.");
                return AdamResponse.AdamDown;
            }

            return AdamResponse.Unknown;
        }

        #region Global Uplift

        public AdamResponse GlobalUplift(GlobalUpliftTransaction transaction, AdamSettings adamSettings)
        {
            if (!transaction.WriteLine && !transaction.WriteHeader)
            {
                throw new ArgumentException("Invalid GlobalUpliftTransaction - Lines to write are not specified");
            }
            
            AdamResponse result = AdamResponse.Success;
            if (transaction.WriteLine)
            {
                result = WriteGlobalUpliftLine(transaction, adamSettings);
            }

            //If transaction specifies to write header and previous response was success
            if (transaction.WriteHeader && result == AdamResponse.Success)
            {
                result = WriteGlobalUpliftHeader(transaction, adamSettings);
            }

            if (result != AdamResponse.Success)
            {
                // Whether transaction should write line and was successfully written
                var writeLine = transaction.WriteLine && !transaction.LineDidWrite;
                // Whether transaction should write header and  was successfully written
                var writeHeader = transaction.WriteHeader && !transaction.HeaderDidWrite;

                var upliftEvent = new GlobalUpliftEvent
                {
                    Id = transaction.Id,
                    BranchId = transaction.BranchId,
                    AccountNumber = transaction.AccountNumber,
                    CreditReasonCode = transaction.CreditReasonCode,
                    Quantity = transaction.Quantity,
                    ProductCode = transaction.ProductCode,
                    StartDate = transaction.StartDate,
                    EndDate = transaction.EndDate,
                    WriteLine = writeLine,
                    WriteHeader = writeHeader
                };

                // Insert uplift event
                eventRepository.InsertEvent(EventAction.GlobalUplift, upliftEvent);
            }

            return result;
        }

        public virtual AdamResponse WriteGlobalUpliftLine(GlobalUpliftTransaction transaction, AdamSettings adamSettings)
        {
            string sql = _globalUpliftTransactionFactory.LineSql(transaction);
            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();
                    using (var command = new AdamCommand(connection))
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }

                    // Set result and return success
                    transaction.LineDidWrite = true;
                    return AdamResponse.Success;
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred writing global uplift line!", adamException);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, adamException);

                    if (adamException.AdamErrorId == AdamError.ADAMNOTRUNNING)
                    {
                        return AdamResponse.AdamDown;
                    }

                    return AdamResponse.Unknown;
                }
                catch (Exception e)
                {
                    this.logger.LogError("ADAM error occurred writing global uplift line!", e);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, e);
                    return AdamResponse.Unknown;
                }
            }
        }

        public virtual AdamResponse WriteGlobalUpliftHeader(GlobalUpliftTransaction transaction, AdamSettings adamSettings)
        {
            string sql = _globalUpliftTransactionFactory.HeaderSql(transaction);
            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();
                    using (var command = new AdamCommand(connection))
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }

                    // Set result and return success
                    transaction.HeaderDidWrite = true;
                    return AdamResponse.Success;
                }
                catch (AdamProviderException adamException)
                {
                    this.logger.LogError("ADAM error occurred writing global uplift header!", adamException);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, adamException);

                    if (adamException.AdamErrorId == AdamError.ADAMNOTRUNNING)
                    {
                        return AdamResponse.AdamDown;
                    }

                    return AdamResponse.Unknown;
                }
                catch (Exception e)
                {
                    this.logger.LogError("ADAM error occurred writing global uplift line!", e);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, e);
                    return AdamResponse.Unknown;
                }
            }
        }

        #endregion Global Uplift
        /*     public AdamResponse CreditForPod(Job job, AdamSettings adamSettings, int branchId)
             {

                 return AdamResponse.Unknown;
             }

             public AdamResponse CleanPod(Job job, AdamSettings adamSettings, int branchId)
             {
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
                                     "INSERT INTO WELLHEAD (WELLHDGUID, WELLHDCREDAT, WELLHDCRETIM, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDPODCODE, WELLHDCRDNUMREAS, WELLHDLINECOUNT) " +
                                     "VALUES({0}, '{1}', '{2}', {3}, '{4}', {5}, {6}, {7}, {8}, {9}, {10});", job.Id, today, now, (int)EventAction.Pod, "WELL", branchId, acno, job.InvoiceNumber, job.ProofOfDelivery, 0, 0);

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
                     return AdamResponse.Unknown;
                 }
             }*/


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