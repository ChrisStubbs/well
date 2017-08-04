namespace PH.Well.Domain
{
    using System.Reflection;
    using System.Text;

    public class Account : Entity<int>
    {
        public string Code { get; set; }

        public string AccountTypeCode { get; set; }

        public int DepotId { get; set; }

        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string PostCode { get; set; }

        public string ContactName { get; set; }

        public string ContactNumber { get; set; }

        public string ContactNumber2 { get; set; }

        public string ContactEmailAddress { get; set; }

        public int StopId { get; set; }

        private PropertyInfo[] _PropertyInfos = null;

        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }

        public int? LocationId { get; set; }
    }
}

