namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    using PH.Well.Common.Extensions;

    public class PodTransactionFactory : IPodTransactionFactory
    {
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IAccountRepository accountRepository;

        public PodTransactionFactory(IAccountRepository accountRepository,
             IJobDetailRepository jobDetailRepository)
        {
            this.accountRepository = accountRepository;
            this.jobDetailRepository = jobDetailRepository;
        }

        public PodTransaction Build(Job job, int branchId)
        {
            var account = this.accountRepository.GetAccountByStopId(job.StopId);

            var endFlag = 0;
            var acno = (int) (Convert.ToDecimal(job.PhAccount)*1000);
            var today = DateTime.Now.ToString("dd/MM/yyyy");
            var now = DateTime.Now.ToShortTimeString();
            var contact = TruncateString(account.ContactName, 16);
            var wellName = "The Well";

            var source = 0;
            var lineCount = 0;

            var lineDictionary = new Dictionary<int, string>();

            var podLines = GetPodDeliveryLineCredits(job.Id, (int)job.JobStatus).ToList();

            var podCount = podLines.Count();

            foreach (var line in podLines)
            {
                lineCount++;
                if (lineCount == podCount)
                {
                    endFlag = 1;
                }
                var podCreditLine =
                    string.Format(
                        "INSERT INTO WELLLINE(WELLINEGUID, WELLINERCDTYPE ,WELLINESEQNUM, WELLINEPODREASON, WELLINEQTY, WELLINEPROD, WELLINEENDLINE) VALUES({0}, {1},' {2} ', {3}, {4}, {5}, {6});",
                        job.Id, (int) EventAction.Pod, lineCount, line.Reason, line.Quantity, line.ProductCode, endFlag);

                lineDictionary.Add(lineCount, podCreditLine);
            }

            var creditHeader = string.Format(
                "INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDCRETIM, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDSRCERROR, WELLHDFLAG, WELLHDPODCODE, WELLHDCONTACT, WELLHDLINECOUNT, WELLHDCUSTREF) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, {8}, {9}, '{10}', '{11}', {12}, '{13}');",
                today, now, job.Id, (int) EventAction.Pod, "Well", branchId, acno, job.InvoiceNumber, source, 0, job.ProofOfDelivery, contact, lineCount, wellName);

            var podTransaction = new PodTransaction
            {
                HeaderSql = creditHeader,
                LineSql = lineDictionary,
                BranchId = branchId,
                JobId = job.Id
            };

            return podTransaction;
        }

        public IEnumerable<PodDeliveryLineCredit> GetPodDeliveryLineCredits(int jobId, int jobStatus)
        {
            var jobDetails = this.jobDetailRepository.GetByJobId(jobId);
            var podLines = new List<PodDeliveryLineCredit>();

            foreach (var line in jobDetails)
            {
                // is it short, is it damaged, or bypassed?
                // if so create the pod line

                if (line.JobDetailDamages.Any())
                {
                    foreach (var damage in line.JobDetailDamages)
                    {
                        var podLine = new PodDeliveryLineCredit { JobId = jobId, Reason = (int)PodReason.Damaged, ProductCode = line.PhProductCode, Quantity = line.DeliveredQty };
                        podLines.Add(podLine);
                    }
                }

                if (line.ShortQty > 0 || jobStatus == (int)JobStatus.Bypassed)
                {
                   var podLine = new PodDeliveryLineCredit { JobId = jobId, Reason = (int)PodReason.DeliveryFailure, ProductCode = line.PhProductCode, Quantity = line.DeliveredQty };
                    podLines.Add(podLine);
                }
            }

            return podLines;
        }

        private string TruncateString(string input, int maxLength)
        {
            if (!string.IsNullOrEmpty(input) && input.Length > maxLength)
            {
                return input.Substring(0, maxLength);
            }

            return input;
        }

    }
}
