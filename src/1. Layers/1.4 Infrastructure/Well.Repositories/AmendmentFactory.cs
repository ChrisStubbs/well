namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common.Extensions;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class AmendmentFactory
    {
        private readonly IUserRepository userRepository;

        public AmendmentFactory(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public AmendmentTransaction Build(Amendment amendment)
        {
            var user = this.userRepository.GetByIdentity(amendment.AmenderName);
            var initials = user.FriendlyName.GetInitials();

            var acno = (int)(Convert.ToDecimal(amendment.AccountNumber) * 1000);
            var today = DateTime.Now.ToShortDateString();
            var now = DateTime.Now.ToShortTimeString();
            var lineCount = 0;
            var lineDictionary = new Dictionary<int, string>();
            var totalOfLines = amendment.AmendmentLines.Count;
            var endFlag = 0;

            foreach (var line in amendment.AmendmentLines)
            {
                lineCount++;
                if (lineCount == totalOfLines)
                {
                    endFlag = 1;
                }

                var amendLine =
                    $"INSERT INTO WELLLINE.WELLNAMDREC (WELLNAMDGUID, WELLNAMDRCDTYPE, WELLNAMDSEQNUM, WELLNAMDQTDELDEL, WELLNAMDQTDELSHT, WELLNAMDQTDELDAM, WELLNAMDQTDELREJ, WELLNAMDQTAMDDEL, WELLNAMDQTAMDSHT, WELLNAMDQTAMDDAM, WELLNAMDQTAMDREJ, WELLNAMDENDLINE) VALUES( {line.JobId}, {(int)EventAction.Amendment}, {lineCount}, {line.DeliveredQuantity}, {line.ShortTotal}, {line.DamageTotal}, {line.RejectedTotal}, {line.AmendedDeliveredQuantity}, {line.AmendedShortTotal}, {line.AmendedDamageTotal}, {line.AmendedRejectedTotal}, {endFlag});";

                lineDictionary.Add(lineCount, amendLine);
            }

            var header =
                $"INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDCRETIM, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDLINECOUNT) VALUES('{today}', '{now}', '{amendment.JobId}', {(int)EventAction.Amendment}, '{initials}', {amendment.BranchId}, {acno}, {amendment.InvoiceNumber}, {lineCount});";

            var amendmentTransaction = new AmendmentTransaction
            {
                HeaderSql = header,
                LineSql = lineDictionary,
                BranchId = amendment.BranchId
            };

            return amendmentTransaction;

        }
    }
}
