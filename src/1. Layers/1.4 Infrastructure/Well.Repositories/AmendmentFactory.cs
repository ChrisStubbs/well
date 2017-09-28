namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common.Extensions;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class AmendmentFactory : IAmendmentFactory
    {
        private readonly IUserRepository userRepository;

        public AmendmentFactory(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public AmendmentTransaction Build(Amendment amendment)
        {
            var user = this.userRepository.GetByIdentity(amendment.AmenderName);
            var initials = user.Name.GetInitials();

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


                var productCodeNumeric = 0;
                var result = Int32.TryParse(line.ProductCode, out productCodeNumeric);
                if (!result)
                {
                    productCodeNumeric = 0;
                }

                var amendLine =
                    $"INSERT INTO WELLLINE.WELLNAMDREC (WELLNAMDGUID, WELLNAMDRCDTYPE, WELLNAMDSEQNUM, WELLNAMDPROD, WELLNAMDQTDELDEL, WELLNAMDQTDELSHT, WELLNAMDQTDELDAM, WELLNAMDQTDELREJ, WELLNAMDQTAMDDEL, WELLNAMDQTAMDSHT, WELLNAMDQTAMDDAM, WELLNAMDQTAMDREJ, WELLNAMDENDLINE) VALUES( {line.JobId}, {(int)EventAction.Amendment}, {lineCount}, {productCodeNumeric},  {line.DeliveredQuantity}, {line.ShortTotal}, {line.DamageTotal}, {line.RejectedTotal}, {line.AmendedDeliveredQuantity}, {line.AmendedShortTotal}, {line.AmendedDamageTotal}, {line.AmendedRejectedTotal}, {endFlag});";

                lineDictionary.Add(lineCount, amendLine);
            }

            var header =
                $"INSERT INTO WELLHEAD (WELLHDCREDAT, WELLHDCRETIM, WELLHDGUID, WELLHDRCDTYPE, WELLHDOPERATOR, WELLHDBRANCH, WELLHDACNO, WELLHDINVNO, WELLHDLINECOUNT, WELLHDCUSTREF) VALUES('{today}', '{now}', '{amendment.JobId}', {(int)EventAction.Amendment}, '{initials}', {amendment.BranchId}, {acno}, {amendment.InvoiceNumber.Truncate(9)}, {lineCount}, '{amendment.CustomerReference.Truncate(9)}');";

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
