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

    public class CreditTransactionFactory : ICreditTransactionFactory
    {
        private readonly IJobRepository jobRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserNameProvider userNameProvider;

        public CreditTransactionFactory(IJobRepository jobRepository,
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            IUserNameProvider userNameProvider)
        {
            this.jobRepository = jobRepository;
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
            this.userNameProvider = userNameProvider;
        }

        public CreditTransaction Build(List<DeliveryLineCredit> deliveryLines, int branchId)
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            var initials = user.FriendlyName.GetInitials();
            var wellName = "The Well";

            var job = this.jobRepository.GetById(deliveryLines[0].JobId);
            var account = this.accountRepository.GetAccountByStopId(job.StopId);

            var endFlag = 0;
            var acno = (int)(Convert.ToDecimal(job.PhAccount) * 1000);
            var today = DateTime.Now.ToShortDateString();
            var now = DateTime.Now.ToShortTimeString();

            var totalOfLines = deliveryLines.Count;
            var source = 0;
            var lineCount = 0;
            var groupCount = 0;

            // group credit lines by reason
            var reasonLines = deliveryLines.GroupBy(line => line.Reason);

            var lineDictionary = new Dictionary<int, string>();

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
                    var creditLine =
                        $"INSERT INTO WELLLINE(WELLINEGUID, WELLINERCDTYPE ,WELLINESEQNUM, WELLINECRDREASON, WELLINEQTY, WELLINEPROD, WELLINEENDLINE) VALUES({job.Id}, {(int)EventAction.Credit},' {lineCount} ', {line.Reason}, {line.Quantity}, {line.ProductCode}, {endFlag});";

                    lineDictionary.Add(lineCount, creditLine);
                }
            }

            var creditHeader =
                $"INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDCRETIM, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDSRCERROR, WELLHDFLAG, WELLHDCONTACT, WELLHDLINECOUNT, WELLHDCRDNUMREAS, WELLHDCUSTREF) VALUES('{today}', '{now}', '{job.Id}', '{(int)EventAction.Credit}', '{initials}', {branchId}, {acno}, {job.InvoiceNumber}, {source}, {0}, '{account.ContactName}', {lineCount}, {groupCount}, '{wellName}');";

            var creditTransaction = new CreditTransaction
            {
                HeaderSql = creditHeader,
                LineSql = lineDictionary,
                BranchId = branchId
            };

            return creditTransaction;
        }
    }
}
