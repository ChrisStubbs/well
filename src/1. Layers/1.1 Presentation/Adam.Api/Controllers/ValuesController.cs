namespace PH.Adam.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    public class ValuesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "sdghds", "value2" };
        }
    }
}
