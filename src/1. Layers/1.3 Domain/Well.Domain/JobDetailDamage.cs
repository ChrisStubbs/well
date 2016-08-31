using System;

namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;
    using ValueObjects;

    [Serializable()]
    public class JobDetailDamage : Entity<int>
    {
        public JobDetailDamage()
        {
            
        }

        [XmlElement("Qty")]
        public decimal Qty { get; set; }

        [XmlElement("JobDetailID")]
        public string JobDetailCode { get; set; }

        [XmlIgnore]
        public int JobDetailId { get; set; }

        [XmlElement("Reason")]
        public Reason Reason { get; set; }

        [XmlIgnore]
        public DamageReasons DamageReason
        {
            get
            {
                if (Reason != null)
                {
                    return (DamageReasons)Enum.Parse(typeof(DamageReasons), Reason.Code);
                }
                return DamageReasons.Notdef;
            }
            set
            {
                Reason = new Reason()
                {
                    Code = value.ToString(),
                    Description = StringExtensions.GetEnumDescription(value)
                };
            }
        }
    }
}
