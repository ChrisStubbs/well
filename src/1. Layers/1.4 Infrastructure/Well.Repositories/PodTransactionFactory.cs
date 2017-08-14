namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    using PH.Well.Common.Extensions;

    public class PodTransactionFactory : IPodTransactionFactory
    {
        private readonly IAccountRepository accountRepository;

        public PodTransactionFactory(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
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

            var podLines = GetPodDeliveryLineCredits(job).ToList();

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
                        "INSERT INTO WELLLINE.WELLINEREC (WELLINEGUID, WELLINERCDTYPE ,WELLINESEQNUM, WELLINEPODREASON, WELLINEQTY, WELLINEPROD, WELLINEENDLINE) VALUES({0}, {1},' {2} ', {3}, {4}, {5}, {6});",
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

        public IEnumerable<PodDeliveryLineCredit> GetPodDeliveryLineCredits(Job job)
        {
            // PODs send to ADAM the quantity DELIVERED not the quantity SHORT/DAMAGED
            var podLines = new List<PodDeliveryLineCredit>();

            if (job.ProofOfDelivery.GetValueOrDefault() == (int)ProofOfDelivery.CocaCola)
            {
                podLines = GetPodDeliveryLineCreditsForCCE(job).ToList();
            }
            else
            {
                podLines = GetPodDeliveryLineCreditsForLRS(job).ToList();
            }

            return podLines;
        }

        public IEnumerable<PodDeliveryLineCredit> GetPodDeliveryLineCreditsForCCE(Job job)
        {
            var podLines = new List<PodDeliveryLineCredit>();
            
            foreach (var line in job.LineItems)
            {
                var deliveredQuantity = job.JobDetails.Find(x => x.LineItemId == line.Id).DeliveredQty;
                
                foreach (var action in line.LineItemActions)
                {
                    var quantity = 0;
                    quantity = line.AmendedDeliveryQuantity ?? deliveredQuantity;

                    if (action.ExceptionType == ExceptionType.Damage)
                    {
                        var reason = 0;
                        if (!string.IsNullOrWhiteSpace(action.PdaReasonDescription) && action.PdaReasonDescription.ToLower().Replace(" ", string.Empty).Contains("notrequired"))
                            {
                                reason = (int)PodReason.Refused;
                            }
                        else
                            {
                                reason = (int)PodReason.Damaged;
                            }

                        var podLine = new PodDeliveryLineCredit { JobId = job.Id, Reason = reason, ProductCode = line.ProductCode, Quantity = quantity };
                        podLines.Add(podLine);
                    }

                    if (action.ExceptionType == ExceptionType.Short || action.ExceptionType == ExceptionType.Bypass)
                    {
                        var podLine = new PodDeliveryLineCredit { JobId = job.Id, Reason = (int)PodReason.DeliveryFailure, ProductCode = line.ProductCode, Quantity = quantity };
                        podLines.Add(podLine);
                    }
                }
            }

            return podLines;
        }


        public IEnumerable<PodDeliveryLineCredit> GetPodDeliveryLineCreditsForLRS(Job job)
        {
            var podLines = new List<PodDeliveryLineCredit>();

            foreach (var line in job.LineItems)
            {
                var deliveredQuantity = job.JobDetails.Find(x => x.LineItemId == line.Id).DeliveredQty;
                foreach (var action in line.LineItemActions)
                {
                    var quantity = 0;
                    quantity = line.AmendedDeliveryQuantity ?? deliveredQuantity;
                    if (action.ExceptionType == ExceptionType.Damage)
                    {
                        var reason = 0;
                        if (!string.IsNullOrWhiteSpace(action.PdaReasonDescription) && action.PdaReasonDescription.ToLower().Replace(" ", string.Empty).Contains("notrequired"))
                        {
                            reason = (int)PodReason.Refused;
                        }
                        else
                        {
                            reason = (int)PodReason.Damaged;
                        }

                        var podLine = new PodDeliveryLineCredit { JobId = job.Id, Reason = reason, ProductCode = line.ProductCode, Quantity = quantity };
                        podLines.Add(podLine);
                    }

                    if (action.ExceptionType == ExceptionType.Short)
                    {
                        var podLine = new PodDeliveryLineCredit { JobId = job.Id, Reason = (int)PodReason.PickingError, ProductCode = line.ProductCode, Quantity = quantity };
                        podLines.Add(podLine);
                    }

                    if (action.ExceptionType == ExceptionType.Bypass)
                    {
                        var podLine = new PodDeliveryLineCredit { JobId = job.Id, Reason = (int)PodReason.UnableToOffload, ProductCode = line.ProductCode, Quantity = quantity };
                        podLines.Add(podLine);
                    }
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
