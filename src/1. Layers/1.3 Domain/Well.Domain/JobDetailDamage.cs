namespace PH.Well.Domain
{
    using System;
    using System.Dynamic;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    using Enums;
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
