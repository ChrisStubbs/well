namespace PH.Well.Domain.ValueObjects
{
    public class LocationAccount
    {
        public int BranchId { get; set; }

        public string AccountCode { get; set; }

        public string Name { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Postcode { get; set; }

        public int StopId { get; set; }

    }
}
