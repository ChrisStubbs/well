using System;

namespace PH.Well.Domain
{
    using System.Xml.Serialization;
    using Common.Extensions;
    using Enums;
    using ValueObjects;

    [Serializable()]
    public class JobDetailDamage : Entity<int>, IEquatable<JobDetailDamage>
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

        [XmlElement("Source")]
        public DamageSource Source { get; set; }

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

        [XmlIgnore]
        public JobDetailDamageSource JobDetailDamageSource
        {
            get
            {
                if (Source != null)
                {
                    var damageReason =  (JobDetailDamageSource)Enum.Parse(typeof(JobDetailDamageSource), Source.Code);

                                  return damageReason;

                }
                return JobDetailDamageSource.NotDef;
            }
            set
            {
                Source = new DamageSource()
                {
                    Code = value.ToString(),
                    Description = StringExtensions.GetEnumDescription(value)
                };
            }
        }

        public string GetDamageString()
        {
            return $"{DamageReason.ToString()} - {Qty}";
        }

        public bool Equals(JobDetailDamage other)
        {
            if (other == null)
            {
                return false;
            }
            return other.DamageReason == DamageReason && other.Qty == Qty;
        }
    }
}
