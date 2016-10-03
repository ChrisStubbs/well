namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Enums;

    public class DeliveryDetailModel
    {
        public DeliveryDetailModel()
        {
            this.DeliveryLines = new List<DeliveryLineModel>();
        }

        public int Id { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public string AccountAddress { get; set; }

        public string InvoiceNumber { get; set; }

        public string ContactName { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

        public string DeliveryType { get; set; } // resolved clean exception

        public bool IsException { get; set; }

        public bool CanAction { get; set; }

        public bool CanSubmit { get {return CanAction && DeliveryLines.Any(dl => dl.Actions.Any(a => a.Status == ActionStatus.Draft)); } }

        public List<DeliveryLineModel> DeliveryLines { get; set; }
    }
}