namespace PH.Well.Domain
{
    using System;

   
   public class Account : Entity<int>
   {
   public class Account:Entity<int>
    {
       public Account()
       {
           
       }
       
        public string Code { get; set; }

        public int AccountTypeCode { get; set; }

        public string Name  { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string PostCode { get; set; }

        public string ContactName { get; set; }

        public string ContactNumber { get; set; }

        public string ContactNumber2 { get; set; }

        public string ContactEmailAddress { get; set; }

        public TimeSpan StartWindow { get; set; }

        public TimeSpan EndWindow { get; set; }

        public string DepotId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
       public KeyValuePair<int, KeyValuePair<int, string>> AccountMetaData { get; set; }

        public bool IsDropAndDrive { get; set; }

        public int StopId { get; set; }
    }
}
