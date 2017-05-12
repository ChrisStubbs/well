namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.Generic;
    using Well.Domain;
    using Well.Domain.Enums;

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

        public JobFactory WithTotalShort(int? value)
        {
            var totshort = new EntityAttribute { Code = "TOTSHORT" };
            var totalShortEntityAttributeValue = new EntityAttributeValue {EntityAttribute = totshort, Value = value.ToString()};
            Entity.EntityAttributeValues.Add(totalShortEntityAttributeValue);
            return this;
        }

        public JobFactory WithCod(string value)
        {
            var codEntityAttribute = new EntityAttribute { Code = "COD", Value = value };
            Entity.EntityAttributes.Add(codEntityAttribute);
            return this;
        }
    }
}
