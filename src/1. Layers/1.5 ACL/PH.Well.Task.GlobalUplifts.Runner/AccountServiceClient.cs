using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.AccountService.Client.Interfaces;
using PH.Shared.AccountService.Client.Rest;
using PH.Shared.AccountService.Models;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    /// <summary>
    /// This is wrapper for AccountServiceClientRest that uses GlobalUpliftRunnerConfig to configure underlying RestAccountServiceClient
    /// </summary>
    public class AccountServiceClient  : IAccountServiceClient
    {
        private RestAccountServiceClient client;

        public AccountServiceClient(GlobalUpliftRunnerConfig config)
        {
            client = new RestAccountServiceClient(config.AccountServiceUrl,
                new HttpClient(new HttpClientHandler {UseDefaultCredentials = true}));
        }

        public IEnumerable<AccountModel> GetAccounts(DateTime? changedAfter = null, string filterBy = null, string orderBy = null,
            int? pageNumber = null, int? pageSize = null)
        {
            return client.GetAccounts(changedAfter, filterBy, orderBy, pageNumber, pageSize);
        }

        public AccountModel GetAccount(int branch, string accountNumber)
        {
            return client.GetAccount(branch, accountNumber);
        }

        public ContactModel AddContact(NewContactModel newContactModel)
        {
            return client.AddContact(newContactModel);
        }

        public bool DeleteContact(Guid contactGuid, Guid? accountGuid)
        {
            return client.DeleteContact(contactGuid, accountGuid);
        }
    }
}
