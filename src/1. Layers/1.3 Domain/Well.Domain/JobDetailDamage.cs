namespace PH.Well.Domain
{
    using System;
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

        [XmlIgnore]
        public string JobDetailCode { get; set; }

        [XmlIgnore]
        public int JobDetailId { get; set; }

        [XmlIgnore]
        public Reason Reason { get; set; }

        [XmlIgnore]
        public DamageSource Source { get; set; }

        [XmlIgnore]
        public JobDetailReason JobDetailReason
        {
            get
            {
                if (Reason != null)
                {
                    return (JobDetailReason)Enum.Parse(typeof(JobDetailReason), Reason.Code);
                }

                return JobDetailReason.NotDefined;
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
        public JobDetailSource JobDetailSource
        {
            get
            {
                if (Source != null)
                {
                    var damageReason =  (JobDetailSource)Enum.Parse(typeof(JobDetailSource), Source.Code);

                                  return damageReason;

                }

                return JobDetailSource.NotDefined;
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
            return $"{this.JobDetailReason.ToString()} - {Qty}";
        }

        public bool Equals(JobDetailDamage other)
        {
            if (other == null)
            {
                return false;
            }

            return other.JobDetailReason == this.JobDetailReason && other.Qty == Qty;
        }
    }
}
