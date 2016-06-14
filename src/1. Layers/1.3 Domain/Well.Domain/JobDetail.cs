namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using Base;

    public class JobDetail:Entity<int>
    {
        public JobDetail()
        {
            this.JobDetailAttributes = new List<JobDetailAttribute>();
        }

        public int LineNumber { get; set; }
        public string BarCode { get; set; }
        public decimal OriginalDispatchQty { get; set; }
        public string ProdDesc { get; set; }
        public int OrderedQty { get; set; }
        public decimal SkuWeight { get; set; }
        public decimal SkuCube { get; set; }
        public string UnitMeasure { get; set; }
        public string TextField1 { get; set; }
        public string TextField2 { get; set; }
        public string TextField3 { get; set; }
        public string TextField4 { get; set; }
        public double SkuGoodsValue  { get; set; }
        public int JobId { get; set; }
        public List<JobDetailAttribute> JobDetailAttributes { get; set; }
    }
}
