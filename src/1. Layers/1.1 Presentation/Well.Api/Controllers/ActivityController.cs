using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    public class ActivityController : ApiController
    {
        // GET: api/Activity
        public ActivitySource Get(string invoiceNumber = null)
        {
            return new ActivitySource
            {
                AccountAddress = "Long AccountAdress with lots of things, and stuff and goodies",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                Assignee = "Assignee",
                Branch = "Branch",
                BranchId = 55,
                Cod = true,
                Date = DateTime.Now,
                ItemNumber = "Invoice",
                IsInvoice = true,
                Pod = true,
                PrimnaryAccount = "slzdjkn",
                Driver = "Driver",
                Tba = 1,
                Details = new List<ActivitySourceDetail>
                {
                    new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 1, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 1, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now           , LineItemId = 8},
                    new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 1, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 1, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(1), LineItemId = 8},
                    new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 1, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 1, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(2), LineItemId = 8},
                    new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 2, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 2, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(3), LineItemId = 8},
                    new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 2, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 2, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(4), LineItemId = 8},
                    new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 2, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 2, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(5), LineItemId = 8},
                    new ActivitySourceDetail {  Checked = true,  Damaged = 2, Description = "Description", Expected = 9, HighValue = true,  JobId = 3, JobType = "cold", Product = "Product", Resolution = "Resolution", Shorts = 1, StopId = 2, ResolutionId = 9, BarCode = "", Type = "good", jobTypeAbbreviation = "g", Value = 36M, StopDate = DateTime.Now.AddDays(6), LineItemId = 8},
                    new ActivitySourceDetail {  Checked = false, Damaged = 2, Description = "Description", Expected = 9, HighValue = false, JobId = 4, JobType = "Hot",  Product = "Bla",     Resolution = "Resolution", Shorts = 1, StopId = 3, ResolutionId = 9, BarCode = "", Type = "bad",  jobTypeAbbreviation = "b", Value = 36M, StopDate = DateTime.Now.AddDays(7), LineItemId = 8}
                }
            };
        }
    }

    public class ActivitySource
    {
        public string Branch { get; set; }
        public int BranchId { get; set; }
        public string PrimnaryAccount { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountAddress { get; set; }
        public bool Pod { get; set; }
        public bool Cod { get; set; }
        public string ItemNumber { get; set; }
        public bool IsInvoice { get; set; }
        public DateTime Date { get; set; }
        public string Driver { get; set; }
        public string Assignee { get; set; }
        public int Tba { get; set; }
        public List<ActivitySourceDetail> Details { get; set; }
    }
    public class ActivitySourceDetail
    {
        public string Product { get; set; }
        public string Type { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int Expected { get; set; }
        public int Damaged { get; set; }
        public int Shorts { get; set; }
        public bool Checked { get; set; }
        public bool HighValue { get; set; }
        public string Resolution { get; set; }
        public int ResolutionId { get; set; }
        public int StopId { get; set; }
        public DateTime StopDate { get; set; }
        public int JobId { get; set; }
        public string JobType { get; set; }
        public string jobTypeAbbreviation { get; set; }
        public int LineItemId { get; set; }
    }
}
