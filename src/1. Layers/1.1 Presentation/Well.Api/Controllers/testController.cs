using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PH.Well.Services.Contracts;

namespace PH.Well.Api.Controllers
{
    public class testController : ApiController
    {
        private readonly IWellCleanUpService wellCleanUpService;

        public testController(IWellCleanUpService wellCleanUpService)
        {
            this.wellCleanUpService = wellCleanUpService;
        }

        public async Task<DateTime> Get()
        {
            await this.wellCleanUpService.SoftDelete();

            return DateTime.Now;
        }
    }
}
