using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain.ValueObjects;
    using Services.Contracts;

    public class AppSearchController : ApiController
    {
        private readonly IAppSearchService appSearchService;

        public AppSearchController(IAppSearchService appSearchService)
        {
            this.appSearchService = appSearchService;
        }

        public AppSearchResult Get([FromUri]AppSearchParameters parameters)
        {
            return appSearchService.GetAppSearchResult(parameters);
        }

        //public Test Get([FromUri]AppSearchParameters parameters)
        //{
        //    var testItems = Enumerable.Range(1, 25).Select(x => new
        //        TestItem
        //    {
        //        Type = (x % 2 == 0) ? "invoice" : "location",
        //        Description = $"Search item numero {x}",

        //    }).ToList();

        //    return new Test
        //    {
        //        BranchId = 99999,
        //        Items = testItems
        //    };
        //}

        public class Test
        {
            public int BranchId { get; set; }

            public IEnumerable<TestItem> Items { get; set; }
        }

        public class TestItem
        {
            public string Type { get; set; }
            public string Description { get; set; }

            public object Data { get; set; }
        }
    }
}
