﻿namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class CreditEventTransactionFactory : ICreditEventTransactionFactory
    {
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IUserRepository userRepository;

        public CreditEventTransactionFactory(IJobRepository jobRepository, IJobDetailRepository jobDetailRepository, IAccountRepository accountRepository, IUserRepository userRepository)
        {
             this.jobRepository = jobRepository;
             this.jobDetailRepository = jobDetailRepository;
             this.accountRepository = accountRepository;
             this.userRepository = userRepository;
        }


        public CreditEventTransaction BuildCreditEventTransaction(CreditEvent credit, string username)
        {
            var user = this.userRepository.GetByIdentity(username);
            string initials = new string(user.FriendlyName.ToCharArray().Where(char.IsUpper).ToArray());

            var job = this.jobRepository.GetById(credit.Id);
            var details = this.jobDetailRepository.GetJobDetailsWithActions(credit.Id, 1);
            var account = this.accountRepository.GetAccountGetByAccountCode(job.PhAccount, job.StopId);

            var endFlag = 0;
            var acno = (int)(Convert.ToDecimal(job.PhAccount) * 1000);
            var today = DateTime.Now.ToShortDateString();
            var now = DateTime.Now.ToShortTimeString();
            var jobDetails = details.ToList();

            var totalOfLines = jobDetails.Count;
            var source = 0;
            var lineCount = 0;
            var groupCount = 0;

            // group credit lines by reason
            var reasonLines =
                from line in jobDetails
                group line by line.Reason;

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
                        string.Format(
                            "INSERT INTO WELLLINE(WELLINEGUID, WELLINERCDTYPE ,WELLINESEQNUM, WELLINECRDREASON, WELLINEQTY, WELLINEPROD, WELLINEENDLINE) VALUES({0}, {1},' {2} ', {3}, {4}, {5}, {6});",
                            job.Id, (int)EventAction.CreditTransaction, lineCount, line.Reason, line.Quantity, line.ProductCode, endFlag);

                    lineDictionary.Add(lineCount, creditLine);
                }
            }

            var creditHeader = string.Format(
                "INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDCRETIM, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDSRCERROR, WELLHDFLAG, WELLHDCONTACT, WELLHDCUSTREF, WELLHDLINECOUNT, WELLHDCRDNUMREAS) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, {8}, {9}, '{10}', '{11}', {12}, {13});",
                today, now, job.Id, (int)EventAction.CreditTransaction, initials, credit.BranchId, acno, job.InvoiceNumber, source, 0, account.ContactName, job.CustomerRef, lineCount, groupCount);

            var creditTransaction = new CreditEventTransaction { HeaderSql = creditHeader };

            creditTransaction.LineSql = lineDictionary;

            creditTransaction.BranchId = credit.BranchId;

            return creditTransaction;
        }
    }
}
