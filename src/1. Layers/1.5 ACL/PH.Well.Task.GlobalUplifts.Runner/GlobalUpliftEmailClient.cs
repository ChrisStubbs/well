using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.EmailService.Client.Rest;
using PH.Shared.EmailService.Interfaces;
using PH.Shared.EmailService.Models;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    public class GlobalUpliftEmailClient : IGlobalUpliftEmailService 
    {
        private GlobalUpliftEmailServiceRestClient client;

        public GlobalUpliftEmailClient(GlobalUpliftRunnerConfig config)
        {
            client = new GlobalUpliftEmailServiceRestClient(config.GlobalUpliftEmailServiceUrl,
                new HttpClient(new HttpClientHandler {UseDefaultCredentials = true}));
        }

        public void SendGlobalUpliftEmail(GlobalUpliftEmailData data)
        {
            client.SendGlobalUpliftEmail(data);
        }
    }
}
