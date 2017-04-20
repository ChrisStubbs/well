
namespace PH.Well.Api.Controllers
{
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Extensions;
    using Domain.Enums;

    public class JobTypeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var result = Enum<JobType>.GetValuesAndDescriptions()
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