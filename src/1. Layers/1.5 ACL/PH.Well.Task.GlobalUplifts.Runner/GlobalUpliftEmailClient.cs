using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.EmailService.Client.Rest;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    public class GlobalUpliftEmailClient : GlobalUpliftEmailServiceRestClient
    {
        public GlobalUpliftEmailClient() : base(@"http:\\localhost/emailservice/api/globaluplift/", new HttpClient())
        {
        }
    }
}
