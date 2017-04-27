using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    public class DriverController : ApiController
    {
        public List<string> Get()
        {
            return new List<string>
            {
                "HALL IAN",
                "RENTON MARK",
                "DUGDALE STEVEN"
            };
        }
    }
}
