using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.EmailService.Interfaces;
using PH.Shared.EmailService.Models;
using PH.Shared.Well.Data.EF;
using PH.Well.Services;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    /// <summary>
    /// Send any unsent, complete, Global Uplift attempt emails
    /// </summary>
    public class SendBranchEmailProcessor : ITaskProcessor
    {
        #region Private fields
        private const string NOT_AVAILABLE = "*Not available*";
        private const string ALL_BRANCHES_EMAIL = "all_branches@palmerharvey.co.uk";
        private readonly WellEntities _wellEntities;
        private readonly IGlobalUpliftEmailService globalUpliftEmailService;

        #endregion Private fields

        #region Constructors
        public SendBranchEmailProcessor(WellEntities wellEntities, IGlobalUpliftEmailService globalUpliftEmailService)
        {
            _wellEntities = wellEntities;
            this.globalUpliftEmailService = globalUpliftEmailService;
        }
        #endregion Constructors

        #region Public methods
        public void Run()
        {
            var globalUpliftAttempts = _wellEntities.GlobalUpliftAttempt.Include("GlobalUplift").Where(x => x.DateBranchEmailSent == null && x.GlobalUplift.HasAccountInfo);
            foreach (var globalUpliftAttempt in globalUpliftAttempts)
            {
                // Send global uplift email
                SendGlobalUpliftEmail(globalUpliftAttempt,globalUpliftAttempt.GlobalUplift);
                globalUpliftAttempt.DateBranchEmailSent = DateTime.Now;
            }

            // Update attempts
            _wellEntities.SaveChanges();
        }


        #endregion Public methods

        public void SendGlobalUpliftEmail(GlobalUpliftAttempt attempt, GlobalUplift globalUplift)
        {
            var branchSettings = AdamSettingsFactory.GetAdamSettings((Domain.Enums.Branch)globalUplift.BranchId);

            Console.WriteLine($"Sending email {globalUplift.BranchId}/{globalUplift.PHAccount}");
            var addressLines = (globalUplift.AddressLines + "\n\n\n\n").Split(new char[] { '\n', '\r' },
                StringSplitOptions.None);

            var globalUpliftEmail = new GlobalUpliftEmailData()
            {
                BranchNumber = globalUplift.BranchId.ToString(),
                AccountNumber = globalUplift.PHAccount,
                BoxCount = attempt.CollectedQty.GetValueOrDefault().ToString(),
                AccountName = globalUplift.AccountName,
                Address1 = addressLines[0],
                Address2 = addressLines[1],
                Address3 = addressLines[2],
                Reference = globalUplift.CsfReference,
                TradingAs = globalUplift.AccountName ?? NOT_AVAILABLE,
                BranchName = ((PH.Well.Domain.Enums.Branch)globalUplift.BranchId).ToString(),
                CollectionDate = attempt.DateAttempted.ToString("dd MMM yyyy"),
                ContactName = globalUplift.ContactName,
                Telephone = globalUplift.ContactNumber,
                Subject =
                    $"Global Uplift: BR {globalUplift.BranchId:00} Acct {globalUplift.PHAccount} Ref {globalUplift.CsfReference}",
                To = new List<string> { branchSettings.EmailAddress ?? "david.johnston@palmerharvey.co.uk" },
                From = ALL_BRANCHES_EMAIL
            };
            globalUpliftEmailService.SendGlobalUpliftEmail(globalUpliftEmail);
        }

    }
}
