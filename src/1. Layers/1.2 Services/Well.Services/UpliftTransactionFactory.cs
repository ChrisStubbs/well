namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;

    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;

    using Repositories.Contracts;

    public class UpliftTransactionFactory : IUpliftTransactionFactory
    {
        private readonly IJobRepository jobRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserNameProvider userNameProvider;

        public UpliftTransactionFactory(IJobRepository jobRepository,
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            IUserNameProvider userNameProvider)
        {
            this.jobRepository = jobRepository;
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
            this.userNameProvider = userNameProvider;
        }

        // FIRST PASS - need to check with Pete what is required
        // this is copied from the CreditTransactionFactory

        public CreditTransaction Build(List<DeliveryLineUplift> deliveryLines, int branchId)
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);
            //ADAM needs the user initials & well identifier
            var initials = user.Name.GetInitials();
            var wellName = "The Well";

            var job = this.jobRepository.GetById(deliveryLines[0].JobId);
            var account = this.accountRepository.GetAccountByStopId(job.StopId);

            var endFlag = 0;
            var acno = (int)(Convert.ToDecimal(job.PhAccount) * 1000);
            var today = DateTime.Now.ToShortDateString();
            var now = DateTime.Now.ToShortTimeString();

            // only send the lines for credit.  If there are no lines for credit, just close only send the header
            var linesForAdam = deliveryLines.Where(x => x.Action == DeliveryAction.Credit).ToList();
            var totalOfLines = linesForAdam.Count();
            var source = 0;
            var lineCount = 0;

            var lineDictionary = new Dictionary<int, string>();

            foreach (var line in linesForAdam)
            {
                lineCount++;
                if (lineCount == totalOfLines)
                {
                    endFlag = 1;
                    source = line.Source;
                }

                var creditLine =
                    $"INSERT INTO WELLLINE.WELLINEREC (WELLINEGUID, WELLINERCDTYPE ,WELLINESEQNUM, WELLINECRDREASON, WELLINEQTY, WELLINEPROD, WELLINEENDLINE) VALUES({job.Id}, {(int) EventAction.StandardUplift},' {lineCount} ', {line.Reason}, {line.Quantity}, {line.ProductCode}, {endFlag});";

                lineDictionary.Add(lineCount, creditLine);
            }

            // NB - for Uplifts, use the 'PickListRef' for InvoiceNumber to ADAM - it is actually the CSF number for an uplift
            var creditHeader =
                $"INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDCRETIM, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDSRCERROR, WELLHDFLAG, WELLHDCONTACT, WELLHDLINECOUNT, WELLHDCUSTREF) VALUES('{today}', '{now}', '{job.Id}', '{(int)EventAction.StandardUplift}', '{initials}', {branchId}, {acno}, {job.PickListRef}, {source}, {0}, '{account.ContactName}', {lineCount}, '{wellName}');";

            var creditTransaction = new CreditTransaction
            {
                HeaderSql = creditHeader,
                LineSql = lineDictionary,
                BranchId = branchId,
                JobId = job.Id
            };

            return creditTransaction;
        }
    }
}
