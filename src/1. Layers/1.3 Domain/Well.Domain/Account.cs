namespace PH.Well.Domain
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

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
    }

    [Serializable()]
    public class AccountDTO
    {
        public int Id { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("AccountTypeCode")]
        public string AccountTypeCode { get; set; }

        [XmlElement("DepotID")]
        public int DepotId { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Address1")]
        public string Address1 { get; set; }

        [XmlElement("Address2")]
        public string Address2 { get; set; }

        [XmlElement("PostCode")]
        public string PostCode { get; set; }

        [XmlElement("ContactName")]
        public string ContactName { get; set; }

        [XmlElement("ContactNumber")]
        public string ContactNumber { get; set; }

        [XmlElement("ContactNumber2")]
        public string ContactNumber2 { get; set; }

        [XmlElement("ContactEmailAddress")]
        public string ContactEmailAddress { get; set; }

        [XmlIgnore]
        public int StopId { get; set; }

        [XmlIgnore]
        public bool IsDeleted { get; set; }
    }
}

