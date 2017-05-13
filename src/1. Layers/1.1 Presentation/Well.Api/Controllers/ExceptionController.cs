using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    public class ExceptionController : ApiController
    {
        public IList<EditLineItemException> PerLineItem([FromUri]int[] id)
        {
            var result = new List<EditLineItemException>
            {
                new EditLineItemException { Id = 1, Originator = "Driver", ProductNumber = "65651", Product = "Snickers", Exception = "Damaged", Invoiced = 2, Delivered = 2, Quantity = 10, Action = "Redeliver", Source = null, Reason = null, Erdd = DateTime.Now.AddDays(2), ActionedBy = "Me", ApprovedBy = "No One" },
                new EditLineItemException { Id = 1, Originator = "Client", ProductNumber = "65651", Product = "Snickers", Exception = "Short",   Invoiced = 0, Delivered = 2, Quantity = 33, Action = "Credit",    Source = null, Reason = null, Erdd = null,                    ActionedBy = "Me", ApprovedBy = "No One" },
                new EditLineItemException
                {
                    Id = 2, Originator = "Client", ProductNumber = "11114", Product = "Mars", Exception = "Short", Invoiced = 0, Delivered = 2, Quantity = 33, Action = "Credit", Source = null, Reason = null, Erdd = null, ActionedBy = "Me", ApprovedBy = "No One",
                    Comments = new List<string> { "Change 1 ", "Change 2"}
                }
            };

            return result;
        }
    }

    public class EditLineItemException
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string Product { get; set; }
        public string Originator { get; set; }
        public string Exception { get; set; }
        public int? Invoiced { get; set; }
        public int? Delivered { get; set; }
        public int Quantity { get; set; }
        public string Action { get; set; }
        public string Source { get; set; }
        public string Reason { get; set; }
        public DateTime? Erdd { get; set; }
        public string ActionedBy { get; set; }
        public string ApprovedBy { get; set; }
        public IList<string> Comments { get; set; }
    }
}
