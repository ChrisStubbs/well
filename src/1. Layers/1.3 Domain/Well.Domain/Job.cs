namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    

    public class Job:Entity<int>
    {
        public Job()
        {
           
        }

        public int Sequence { get; set; }
        public string JobTypeCode { get; set; }
        public string JobRef1 { get; set; }
        public string JobRef2 { get; set; }
        public string JobRef3 { get; set; }
        public string JobRef4 { get; set; }
        public DateTime OrderDate { get; set; }
        public string Originator { get; set; }
        public string TextField1 { get; set; }
        public string TextField2 { get; set; }
        public int StopId { get; set; }
        public KeyValuePair<int, KeyValuePair<int, string>> JobMetaData { get; set; }

    }
}
