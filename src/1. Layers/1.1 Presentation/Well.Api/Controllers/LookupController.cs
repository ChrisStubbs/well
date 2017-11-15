using System;
using System.Collections.Generic;
using System.Web.Http;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;
using System.Linq;

namespace PH.Well.Api.Controllers
{
    public class LookupController : ApiController
    {
        private readonly ILookupService lookupService;
        public LookupController(ILookupService lookupService)
        {
            this.lookupService = lookupService;
        }

        //note: This will use the default database connection as no branchId prefix is supplied in route
        [Route("Lookup/{lookUp}")]
        public IList<KeyValuePair<string, string>> Get(string lookUp)
        {
            LookupType lookupValue;

            lookupValue = (LookupType)Enum.Parse(typeof(LookupType), lookUp, true);
            if (!(Enum.IsDefined(typeof(LookupType), lookupValue) | lookupValue.ToString().Contains(",")))
            {
                throw new ArgumentException($"{lookUp}");
            }

            return lookupService.GetLookup(lookupValue);
        }
    }
}
