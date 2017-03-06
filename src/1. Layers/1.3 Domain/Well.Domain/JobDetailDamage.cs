namespace PH.Well.Domain
{
    using System;
    using System.Xml.Serialization;

    using Enums;

    using PH.Well.Domain.Extensions;

    using ValueObjects;

    using StringExtensions = PH.Well.Common.Extensions.StringExtensions;

    [Serializable()]
    public class JobDetailDamage : Entity<int>, IEquatable<JobDetailDamage>
    {
        public JobDetailDamage()
        {
            this.Source = new DamageSource();
            this.Reason = new Reason();
        }

        [XmlIgnore]
        public int DamageActionId { get; set; }

        public DeliveryAction DamageAction => (DeliveryAction) DamageActionId;

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

        [XmlElement("Reason")]
        public Reason Reason { get; set; }

        [XmlElement("Source")]
        public DamageSource Source { get; set; }

        [XmlIgnore]
        public int JobDetailReasonId { get; set; }

        [XmlIgnore]
        public int JobDetailSourceId { get; set; }

        [XmlIgnore]
        public JobDetailStatus DamageStatus { get; set; }

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

        public override string ToString()
        {
            return
                $"Reason - {EnumExtensions.GetDescription((JobDetailReason)this.JobDetailReasonId)}, Source - {EnumExtensions.GetDescription((JobDetailSource)this.JobDetailSourceId)}, Action - {EnumExtensions.GetDescription((DeliveryAction)this.DamageActionId)} - {this.Qty}";
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
