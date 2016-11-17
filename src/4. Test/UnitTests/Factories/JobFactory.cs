namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Well.Domain;
    using Well.Domain.Enums;

    public class JobFactory : EntityFactory<JobFactory, Job>
    {
        public JobFactory()
        {
            this.Entity.Id = 1;
            this.Entity.Sequence = 1;
            this.Entity.JobTypeCode = "001";
            this.Entity.PhAccount = "J001";
            this.Entity.PickListRef = "J002";
            this.Entity.InvoiceNumber = "J0032";
            this.Entity.CustomerRef = "J004";
            this.Entity.OrderDate = DateTime.Now;
            this.Entity.RoyaltyCode = "Royal1";
            this.Entity.RoyaltyCodeDesc = "RoyalDesc";
            //this.Entity.GrnNumber = null;
           // this.Entity.GrnRefusedReason = null;
            this.Entity.GrnRefusedDesc = null;
            this.Entity.PerformanceStatus = PerformanceStatus.Narri;
            this.Entity.ByPassReason = ByPassReasons.Notdef;
            this.Entity.StopId = 1;

            this.Entity.EntityAttributes = new Collection<EntityAttribute>();
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "GrnNumber", Value = null});
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "GrnRefusedReason", Value = null });
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "ActionLogNumber", Value = null });
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "IsOverage", Value = null });
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "OuterCount", Value = null });
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "OuterDiscrepancyFound", Value = null });
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "TotalOutersOver", Value = null });
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "TotalOutersShort", Value = null });

        }
    }
}
