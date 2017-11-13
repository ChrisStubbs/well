using System.Linq;
using PH.Shared.AccountService.Client.Interfaces;
using PH.Shared.Well.Data.EF;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    /// <summary>
    /// Fetch any missing account details for Global Uplifts
    /// </summary>
    public class UpdateAccountDetailsProcessor : ITaskProcessor
    {
        #region Private fields
        private readonly WellEntities _wellEntities;
        private readonly IAccountServiceClient accountService;

        #endregion Private fields

        #region Constructors
        public UpdateAccountDetailsProcessor(WellEntities wellEntities, IAccountServiceClient accountService)
        {
            _wellEntities = wellEntities;
            this.accountService = accountService;
        }
        #endregion Constructors

        #region Public methods
        public void Run()
        {
            var globalUpliftAttempts = _wellEntities.GlobalUpliftAttempt.Include("GlobalUplift")
                .Where(x => x.DateBranchEmailSent == null && x.CollectedQty > 0 && !x.GlobalUplift.HasAccountInfo)
                .ToList();
            foreach (var globalUpliftAttempt in globalUpliftAttempts)
            {
                // Use the Account service to fetch account details for these global uplifts
                var account = accountService.GetAccount(globalUpliftAttempt.GlobalUplift.BranchId,
                    globalUpliftAttempt.GlobalUplift.PHAccount);

                if (account != null)
                {
                    globalUpliftAttempt.GlobalUplift.AccountName = account.AccountName;
                    globalUpliftAttempt.GlobalUplift.AddressLines = account.DeliveryAddress?.AddressLines;
                    globalUpliftAttempt.GlobalUplift.Postcode = account.DeliveryAddress?.Postcode;

                    var primaryContact = account.Contacts.FirstOrDefault(x => x.IsPrimaryContact);
                    if (primaryContact != null)
                    {
                        globalUpliftAttempt.GlobalUplift.ContactName = primaryContact.Name;
                        globalUpliftAttempt.GlobalUplift.ContactNumber = primaryContact.Phone;
                    }

                    if (!string.IsNullOrWhiteSpace(globalUpliftAttempt.GlobalUplift.AccountName) &&
                        !string.IsNullOrWhiteSpace(globalUpliftAttempt.GlobalUplift.AddressLines) &&
                        !string.IsNullOrWhiteSpace(globalUpliftAttempt.GlobalUplift.Postcode))
                    {
                        globalUpliftAttempt.GlobalUplift.HasAccountInfo = true;
                    }
                }
            }

            _wellEntities.SaveChanges();
        }

        #endregion Public methods
    }
}
