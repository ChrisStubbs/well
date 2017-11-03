using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.EmailService.Client.Rest;
using PH.Shared.EmailService.Models;
using PH.Shared.Well.Data.EF;
using PH.Well.Services;
using Branch = PH.Well.Domain.Enums.Branch;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    /// <summary>
    /// Fetch any missing account details for Global Uplifts
    /// </summary>
    public class UpdateAccountDetailsProcessor : ITaskProcessor
    {
        #region Constants
        private const string NOT_AVAILABLE = "*Not available*";
        private const string ALL_BRANCHES_EMAIL = "all_branches@palmerharvey.co.uk";
        #endregion Constants

        #region Private fields
        private readonly WellEntities _wellEntities;
        private readonly GlobalUpliftEmailServiceRestClient _emailServiceRestClient;
        #endregion Private fields

        #region Constructors
        public UpdateAccountDetailsProcessor(WellEntities wellEntities, GlobalUpliftEmailServiceRestClient emailServiceRestClient)
        {
            _wellEntities = wellEntities;
            _emailServiceRestClient = emailServiceRestClient;
        }
        #endregion Constructors

        #region Public methods
        public void Run()
        {
            // Only attempt to send emails, not already sent and that have a non-zero qty collect and valid account info
            var globalUpliftAttempts = _wellEntities.GlobalUpliftAttempt.Include("GlobalUplift").Where(x => x.DateBranchEmailSent == null && x.CollectedQty > 0 && x.GlobalUplift.HasAccountInfo);
            foreach (var globalUpliftAttempt in globalUpliftAttempts)
            {
                // Use the Account service to fetch account details for these global uplifts
                SendGlobalUpliftEmail(globalUpliftAttempt, globalUpliftAttempt.GlobalUplift);
            }
        }
        #endregion Public methods

        #region Private Helper methods
        private void SendGlobalUpliftEmail(GlobalUpliftAttempt attempt, GlobalUplift globalUplift)
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
                BranchName = ((Branch)globalUplift.BranchId).ToString(),
                CollectionDate = attempt.DateAttempted.ToString("dd MMM yyyy"),
                ContactName = globalUplift.ContactName,
                Telephone = globalUplift.ContactNumber,
                Subject =
                    $"Global Uplift: BR {globalUplift.BranchId:00} Acct {globalUplift.PHAccount} Ref {globalUplift.CsfReference}",
                To = new List<string> { branchSettings.EmailAddress ?? "david.johnston@palmerharvey.co.uk" },
                From = ALL_BRANCHES_EMAIL
            };
            _emailServiceRestClient.SendGlobalUpliftEmail(globalUpliftEmail);
        }
        #endregion Private Helper methods
    }
}
