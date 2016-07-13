using System;

namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Enums;

    [Serializable()]
    public class JobDetailDamage : Entity<int>
    {
        public decimal Qty { get; set; }

        [XmlIgnore]
        public int ReasonCategoryId { get; set; }

        [XmlElement("ReasonCategory")]
        public string ReasonCategory
        {
            get { return ReasonCategory; }
            private set { ReasonCategoryId = (int)(ReasonCategory)Enum.Parse(typeof(ReasonCategory), value); }
        }

        [XmlIgnore]
        public int DamageReasonId { get; set; }



        [XmlIgnore]
        public int JobDetailId { get; set; }
    }
}
