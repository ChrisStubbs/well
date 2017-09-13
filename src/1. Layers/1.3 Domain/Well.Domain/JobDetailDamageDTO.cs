using System;
using System.Xml.Serialization;
using PH.Well.Common.Extensions;
using PH.Well.Domain.Enums;
using PH.Well.Domain.Extensions;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.Domain
{
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
}