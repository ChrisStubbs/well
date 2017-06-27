using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    using Domain.ValueObjects;
    using Repositories;
    using Repositories.Contracts;

    public class ActivityController : ApiController
    {
        private readonly IActivityRepository activityRepository;

        public ActivityController(IActivityRepository activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        // GET: api/Activity
        public ActivitySource Get(string invoiceNumber)
        {

            return this.activityRepository.GetActivitySourceByDocumentNumber(invoiceNumber);


            //return new ActivitySource
            //{
            //    AccountAddress = "Long AccountAdress with lots of things, and stuff and goodies",
            //    AccountName = "AccountName",
            //    AccountNumber = "AccountNumber",
            //    Assignee = "Assignee",
            //    Branch = "Branch",
            //    BranchId = 55,
            //    Cod = true,
            //    Date = DateTime.Now,
            //    ItemNumber = "Invoice",
            //    IsInvoice = true,
            //    Pod = true,
            //    PrimaryAccount = "slzdjkn",
            //    Driver = "Driver",
            //    Tba = 1,
            //    Details = new List<ActivitySourceDetail>
            //    {
            //        new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "both",        Expected = 9, HighValue = true,  JobId = 1, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 1, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now           , LineItemId = 8},
            //        new ActivitySourceDetail {  Checked = true,  Damaged = 0, Description = "short",       Expected = 9, HighValue = true,  JobId = 1, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 1, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(1), LineItemId = 8},
            //        new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "damage",      Expected = 9, HighValue = true,  JobId = 1, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 0, StopId = 1, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(2), LineItemId = 8},
            //        new ActivitySourceDetail {  Checked = true,  Damaged = 0, Description = "clean",       Expected = 9, HighValue = true,  JobId = 2, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 0, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(3), LineItemId = 8},
            //        new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 2, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 2, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(4), LineItemId = 8},
            //        new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 2, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 2, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(5), LineItemId = 8},
            //        new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 3, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 2, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(6), LineItemId = 8},
            //        new ActivitySourceDetail {  Checked = false, Damaged = 2, Description = "Description", Expected = 9, HighValue = false, JobId = 4, JobType = "Hot",  Product = "Bla",     Resolution = "Resolution", Shorts = 1, StopId = 3, ResolutionId = 9, BarCode = "", Type = "bad",  jobTypeAbbreviation = "b", Value = 36M, StopDate = DateTime.Now.AddDays(7), LineItemId = 8}
            //    }
            //};
        }
    }

   
}
