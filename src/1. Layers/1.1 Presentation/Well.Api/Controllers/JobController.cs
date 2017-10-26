using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PH.Well.Api.Models.Job;
using PH.Well.Services.Contracts;

namespace PH.Well.Api.Controllers
{
    public class JobController : ApiController
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost]
        public bool SetGrn(SetGrnModel input)
        {
            return _jobService.SetGrn(input.Id, input.Grn);
        }
    }
}
