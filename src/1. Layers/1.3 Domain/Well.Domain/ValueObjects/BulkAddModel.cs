//namespace PH.Well.Domain.ValueObjects
//{
//    using System.Collections.Generic;
//    using Enums;

//    public class BulkAddModel
//    {
//        public BulkAddModel()
//        {
//            this.JobIds = new List<int>();
//            this.LineItemIds = new List<int>();
//        }

//        public IEnumerable<int> JobIds { get; set; }
//        public IEnumerable<int> LineItemIds { get; set; }
//        public BulkAddOptions Option { get; set; }

//        public DeliveryAction DeliverAction { get; set; }
//        public ExceptionType ExceptionType { get; set; }
//        public JobDetailSource Source { get; set; }
//        public JobDetailReason Reason { get; set; }
//    }
//}