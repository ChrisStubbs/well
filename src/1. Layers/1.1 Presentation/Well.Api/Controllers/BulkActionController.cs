//using System.Web.Http;

//namespace PH.Well.Api.Controllers
//{
//    using Domain.ValueObjects;
//    using Services.Contracts;

//    public class BulkActionController : ApiController
//    {
//        private readonly IBulkActionService bulkActionService;

//        public BulkActionController(IBulkActionService bulkActionService)
//        {
//            this.bulkActionService = bulkActionService;
//        }

//        [HttpPatch]
//        public BulkActionResult Add(BulkAddModel bulkAddModel)
//        {
//            return bulkActionService.Add(bulkAddModel);
//        }

//        [HttpGet]
//        [Route("bulkaction/AddSummaryByJob")]
//        public BulkActionSummary GetPreAddSummaryByJobIds([FromUri]int[] id)
//        {
//            if (id == null || id.Length == 0)
//            {
//                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
//            }

//            var bulkAddModel = new BulkAddModel { JobIds = id };
//            return bulkActionService.GetAddSummary(bulkAddModel);
//        }

//        [HttpGet]
//        [Route("bulkaction/AddSummaryByLineItem")]
//        public BulkActionSummary GetPreAddSummaryByLineItemIds([FromUri]int[] id)
//        {
//            if (id == null || id.Length == 0)
//            {
//                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
//            }

//            var bulkAddModel = new BulkAddModel { LineItemIds = id };
//            return bulkActionService.GetAddSummary(bulkAddModel);
//        }
//    }
//}