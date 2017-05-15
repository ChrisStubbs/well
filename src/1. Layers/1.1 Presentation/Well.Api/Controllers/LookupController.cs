using System;
using System.Collections.Generic;
using System.Web.Http;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;

namespace PH.Well.Api.Controllers
{
    public class LookupController : ApiController
    {
        private readonly ILookupService lookupService;
        public LookupController(ILookupService lookupService)
        {
            this.lookupService = lookupService;
        }

        public IList<KeyValuePair<string, string>> Get(string lookUp)
        {
            LookupType lookupValue;

            try
            {
                lookupValue = (LookupType)Enum.Parse(typeof(LookupType), lookUp);
                if (!(Enum.IsDefined(typeof(LookupType), lookupValue) | lookupValue.ToString().Contains(",")))
                    throw new ArgumentException($"{lookUp}");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"{lookUp}");
            }

            return lookupService.GetLookup(lookupValue);
        }
    }
}
