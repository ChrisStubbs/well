namespace PH.Well.Repositories
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

        public AdamRepository(ILogger logger, IJobRepository jobRepository, IEventLogger eventLogger, IPodTransactionFactory podTransactionFactory, IDeliveryReadRepository deliveryReadRepository, IExceptionEventRepository eventRepository)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.deliveryReadRepository = deliveryReadRepository;
            this.eventLogger = eventLogger;
            this.podTransactionFactory = podTransactionFactory;
            this.eventRepository = eventRepository;
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
                        2010);
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
                       2020);
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

                            var commandString =
                                string.Format(
                                    "INSERT INTO WELLHEAD (WELLHDGUID, WELLHDCREDAT, WELLHDCRETIM, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDGRNCODE, WELLHDGRNRCPTREF) " +
                                    "VALUES({0}, '{1}', '{2}', {3}, '{4}', {5}, {6}, {7}, {8}, {9});", grn.Id, today,
                                    now, (int) EventAction.Grn, "WELL", grn.BranchId, acno, delivery.InvoiceNumber,
                                    delivery.GrnProcessType, delivery.GrnNumber);

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
                        2010);
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
                       2020);
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
        

        public AdamResponse Pod(PodEvent podEvent, AdamSettings adamSettings)
        {
            var job = this.jobRepository.GetById(podEvent.Id);
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
                        2010);
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
                       2020);
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

        public AdamResponse GlobalUplift(GlobalUpliftTransaction globalUpliftTransaction, AdamSettings adamSettings)
        {
            
        }

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