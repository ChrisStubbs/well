namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Extensions;
    using Domain.Enums;

    public class JobStatusController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var result = Enum<JobStatus>.GetValuesAndDescriptions()
            .Select(x => new
            {
                id = (int)x.Key,
                description = x.Value
            })
            .ToList();

            return Request.CreateResponse(result); 
        }
    }
}