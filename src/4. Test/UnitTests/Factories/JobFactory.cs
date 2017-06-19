namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.Generic;
    using Well.Domain;
    using Well.Domain.Enums;
    using System.Linq;

    public class JobFactoryDTO : EntityFactory<JobFactoryDTO, JobDTO>
    {
        public JobFactoryDTO()
        {
            Entity.Id = 1;
            Entity.Sequence = 1;
            Entity.JobTypeCode = "001";
            Entity.PhAccount = "J001";
            Entity.PickListRef = "J002";
            Entity.InvoiceNumber = "J0032";
            Entity.CustomerRef = "J004";
            Entity.OrderDate = DateTime.Now;
            Entity.RoyaltyCode = "Royal1";
            Entity.RoyaltyCodeDesc = "RoyalDesc";
            Entity.PerformanceStatus = PerformanceStatus.Narri;
            Entity.JobByPassReason = "some reason";
            Entity.StopId = 1;

            Entity.EntityAttributes = new List<EntityAttribute>();
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "GrnNumber", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "GrnRefusedReason", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "ActionLogNumber", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "IsOverage", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "OuterCount", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "OuterDiscrepancyFound", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "TotalOutersOver", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "TotalOutersShort", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "Picked", Value = null });
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "InvoiceValue", Value = null });
            
            Entity.EntityAttributeValues = new List<EntityAttributeValue>();
        }
        
        public JobFactoryDTO AddEntityAttributeValues(string code, string value)
        {
            var att = this.Entity.EntityAttributeValues
                .FirstOrDefault(p => p.EntityAttribute.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));

            if (att != null)
            {
                this.Entity.EntityAttributeValues.Remove(att);
            }

            this.Entity.EntityAttributeValues.Add(new EntityAttributeValue
            {
                Value = value,
                EntityAttribute = new EntityAttribute { Code = code }
            });

            return this;
        }

        public JobFactoryDTO AddEntityAttribute(string code, string value)
        {
            var att = this.Entity.EntityAttributes
                .FirstOrDefault(p => p.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));

            if (att != null)
            {
                this.Entity.EntityAttributes.Remove(att);
            }

            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = code, Value = value });

            return this;
        }

        public JobFactoryDTO WithTotalShort(int? value)
        {
            var totshort = new EntityAttribute { Code = "TOTSHORT" };
            var totalShortEntityAttributeValue = new EntityAttributeValue {EntityAttribute = totshort, Value = value.ToString()};
            Entity.EntityAttributeValues.Add(totalShortEntityAttributeValue);

            return this;
        }

        public JobFactoryDTO WithCod(string value)
        {
            Entity.EntityAttributes.Add(new EntityAttribute { Code = "COD", Value = value });

            return this;
        }
    }

    public class JobFactory : EntityFactory<JobFactory, Job>
    {
        public JobFactory()
        {
            Entity.Id = 1;
            Entity.Sequence = 1;
            Entity.JobTypeCode = "001";
            Entity.PhAccount = "J001";
            Entity.PickListRef = "J002";
            Entity.InvoiceNumber = "J0032";
            Entity.CustomerRef = "J004";
            Entity.OrderDate = DateTime.Now;
            Entity.RoyaltyCode = "Royal1";
            Entity.RoyaltyCodeDesc = "RoyalDesc";
            Entity.PerformanceStatus = PerformanceStatus.Narri;
            Entity.JobByPassReason = "some reason";
            Entity.Cod = string.Empty;
            Entity.StopId = 1;
            Entity.ResolutionStatus = ResolutionStatus.DriverCompleted;
        }

        public JobFactory WithTotalShort(int? value)
        {
            Entity.TotalOutersShort = value;

            return this;
        }

        public JobFactory WithCod(string value)
        {
            Entity.Cod = value;

            return this;
        }
    }
}
