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

        [XmlIgnore]
        public int Qty { get; set; }

        [XmlElement("Qty")]
        public string QtyXml
        {
            get
            {
                return this.Qty.ToString();                    
            }
            set
            {
                var tryInt = 0;

                if (int.TryParse(value, out tryInt))
                {
                    this.Qty = tryInt;
                }
            }
        }

        [XmlIgnore]
        public string JobDetailCode { get; set; }

        [XmlIgnore]
        public int JobDetailId { get; set; }

        [XmlIgnore]
        public Reason Reason { get; set; }

        [XmlIgnore]
        public DamageSource Source { get; set; }

        // TODO remove this as wont work
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

        // TODO remove this as wont work
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

    [Serializable]
    public class DamageReason
    {
        [XmlElement("ReasonCode")]
        public string ReasonCode { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }

    [Serializable]
    public class Source
    {
        [XmlElement("ReasonCode")]
        public string ReasonCode { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
