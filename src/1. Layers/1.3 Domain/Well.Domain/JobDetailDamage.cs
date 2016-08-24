using System;

namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Enums;

    [Serializable()]
    public class JobDetailDamage : Entity<int>
    {
        public JobDetailDamage()
        {
            this.Reason = DamageReasons.Notdef;
        }

        [XmlElement("Qty")]
        public decimal Qty { get; set; }

        [XmlElement("JobDetailID")]
        public string JobDetailCode { get; set; }

        [XmlIgnore]
        public int JobDetailId { get; set; }

        [XmlElement("Reason")]
        public DamageReasons? Reason { get; set; }
    }
}
