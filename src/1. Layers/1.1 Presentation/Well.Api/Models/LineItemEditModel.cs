using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PH.Well.Api.Models
{
    using Domain.ValueObjects;

    public class LineItemEditModel
    {
        public LineItemEditModel()
        {
            
        }
        public string  Account { get; set; }
        public string  AccountName { get; set; }
        public string ProductNumber { get; set; }
        public string Product { get; set; }
        public int? Invoiced { get; set; }
        public int? Delivered { get; set; }
        public int Quantity { get; set; }

        public IList<LineItemActionUpdate> items;
    }
}