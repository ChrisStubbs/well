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
        public HttpResponseMessage SetGrn(SetGrnModel input)
        {
            try
            {
                _jobService.SetGrn(input.Id, input.Grn);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(new
                {
                    Message = e.Message,
                    customError = true
                }));
            }
        }
    }
}
