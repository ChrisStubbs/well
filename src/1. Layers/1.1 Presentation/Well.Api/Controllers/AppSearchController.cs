using System;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    public class AppSearchController : ApiController
    {
        public AppSearchResult Get(AppSearchParameters parameters)
        {
            return new AppSearchResult
            {
                JobId = 1
            };
        }
    }

    public class AppSearchResult
    {
        public int? JobId { get; set; }
        public int? RouteId { get; set; }
    }

    public class AppSearchParameters
    {
        public int? BrachId { get; set; }
        public DateTime? Date { get; set; }
        public string Account { get; set; }
        public string Invoice { get; set; }
        public string Route { get; set; }
        public string Driver { get; set; }
        public int? DeliveryType { get; set; }
        public int? Status { get; set; }
    }
}
