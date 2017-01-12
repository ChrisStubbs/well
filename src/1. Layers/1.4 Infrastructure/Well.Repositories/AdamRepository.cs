namespace PH.Well.Repositories
{
    using System;
    using System.Linq;
    using AIA.Adam.RFS;
    using AIA.ADAM.DataProvider;
    using Microsoft.Ajax.Utilities;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class AdamRepository : IAdamRepository
    {
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IAccountRepository accountRepository;

        public AdamRepository(ILogger logger, IJobRepository jobRepository, IJobDetailRepository jobDetailRepository, IAccountRepository accountRepository)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.accountRepository = accountRepository;
        }


        public AdamResponse Credit(CreditEvent credit, AdamSettings adamSettings)
        {
            // get the job, jobdetailactions, job details
            var job = this.jobRepository.GetById(credit.Id);
            var details = this.jobDetailRepository.GetJobDetailsWithActions(credit.Id, 1);
            var account = this.accountRepository.GetAccountGetByAccountCode(job.PhAccount, job.StopId);

            var commandString = string.Empty;
            var endFlag = 0;
            var acno = (int)(Convert.ToDecimal(job.PhAccount) * 1000);
            var today = DateTime.Now.ToShortDateString();
            var now = DateTime.Now.ToShortTimeString();
            var jobDetails = details.ToList();

            using (var connection = new AdamConnection(GetConnection(adamSettings)))
            {
                try
                {
                    connection.Open();

                    using (var command = new AdamCommand(connection))
                    {
                        var totalOfLines = jobDetails.Count;
                        var source = 0;
                        var lineCount = 0;
                        var groupCount = 0;

                        // group credit lines by reason
                        var reasonLines =
                            from line in jobDetails
                            group line by line.Reason;

                        foreach (var reasonGroup in reasonLines)
                        {
                            groupCount++;

                            foreach (var line in reasonGroup)
                            {
                                lineCount++;
                                if (lineCount == totalOfLines)
                                {
                                    endFlag = 1;
                                    source = line.Source;
                                }
                                commandString =
                                    string.Format(
                                        "INSERT INTO WELLLINE(WELLINEGUID, WELLINESEQNUM, WELLINECRDREASON, WELLINEQTY, WELLINEPROD, WELLINEENDLINE) VALUES({0}, {1},' {2} ', {3}, {4}, {5});",
                                        job.Id, lineCount, line.Reason, line.Quantity, line.ProductCode, endFlag);
                                command.CommandText = commandString;
                                command.ExecuteNonQuery();
                            }
                        }

                        commandString = string.Format(
        "INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDCRETIM, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDSRCERROR, WELLHDFLAG, WELLHDCONTACT, WELLHDCUSTREF, WELLHDLINECOUNT, WELLHDCRDNUMREAS) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, {8}, {9}, '{10}', '{11}', {12}, {13});",
        today, now, job.Id, 1, "WELL", credit.BranchId, acno, job.InvoiceNumber, source, 0, account.ContactName, job.CustomerRef, lineCount, groupCount);
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