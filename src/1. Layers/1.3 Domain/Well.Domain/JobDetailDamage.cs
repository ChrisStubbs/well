namespace PH.Well.Domain
{
    using System;
    using System.Dynamic;
    using System.Reflection;
    using System.Text;
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

        public int DamageActionId { get; set; }

        public DeliveryAction DamageAction => (DeliveryAction) DamageActionId;

        public int Qty { get; set; }
        
        public string JobDetailCode { get; set; }

        public int JobDetailId { get; set; }

        public Reason Reason { get; set; }

        public DamageSource Source { get; set; }

        public int JobDetailReasonId { get; set; }

        public int JobDetailSourceId { get; set; }

        public JobDetailStatus DamageStatus { get; set; }

        public JobDetailReason JobDetailReason
        {
            get
            {
                if (Reason != null)
                {
                    JobDetailReason jdReason;
                    return Enum.TryParse<JobDetailReason>(Reason.Code, out jdReason)
                        ? jdReason
                        : JobDetailReason.NotDefined;
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

        public bool Equals(JobDetailDamage other)
        {
            if (other == null)
            {
                return false;
            }

            return other.JobDetailReason == this.JobDetailReason && other.Qty == Qty;
        }

        public string PdaReasonDescription { get; set; }

        public JobDetailSource JobDetailSource { get; set; }
    }

    [Serializable()]
    public class JobDetailDamageDTO :  IEquatable<JobDetailDamageDTO>
    {
        public JobDetailDamageDTO()
        {
            this.Source = new DamageSource();

            this.Reason = new Reason();
        }

        [XmlIgnore]
        public int DamageActionId { get; set; }

        public DeliveryAction DamageAction => (DeliveryAction)DamageActionId;

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
                    JobDetailReason jdReason;
                    return Enum.TryParse<JobDetailReason>(Reason.Code, out jdReason)
                        ? jdReason
                        : JobDetailReason.NotDefined;
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
                    JobDetailSource source;
                    return Enum.TryParse<JobDetailSource>(Source.Code, out source)
                        ? source
                        : JobDetailSource.NotDefined;
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

        public bool Equals(JobDetailDamageDTO other)
        {
            if (other == null)
            {
                return false;
            }

            return other.JobDetailReason == this.JobDetailReason && other.Qty == Qty;
        }

        [XmlIgnore]
        public string PdaReasonDescription { get; set; }
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
