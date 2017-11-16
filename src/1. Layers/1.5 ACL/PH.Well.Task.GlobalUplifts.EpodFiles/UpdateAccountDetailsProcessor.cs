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
            var globalUplifts = _wellEntities.GlobalUplift
                .Where(x => !x.HasAccountInfo)
                .ToList();
            foreach (var globalUplift in globalUplifts)
            {
                // Use the Account service to fetch account details for these global uplifts
                var account = accountService.GetAccount(globalUplift.BranchId,
                    globalUplift.PHAccount);

                if (account != null)
                {
                    globalUplift.AccountName = account.AccountName;
                    globalUplift.AddressLines = account.DeliveryAddress?.AddressLines;
                    globalUplift.Postcode = account.DeliveryAddress?.Postcode;

                    var primaryContact = account.Contacts.FirstOrDefault(x => x.IsPrimaryContact);
                    if (primaryContact != null)
                    {
                        globalUplift.ContactName = primaryContact.Name;
                        globalUplift.ContactNumber = primaryContact.Phone;
                    }

                    if (!string.IsNullOrWhiteSpace(globalUplift.AccountName) &&
                        !string.IsNullOrWhiteSpace(globalUplift.AddressLines))
                    {
                        globalUplift.HasAccountInfo = true;
                    }
                }
            }

            _wellEntities.SaveChanges();
        }

        #endregion Public methods
    }
}
