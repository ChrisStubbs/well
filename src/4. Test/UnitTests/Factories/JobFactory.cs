namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.ObjectModel;
    using Well.Domain;
    using Well.Domain.Enums;
    using Attribute = Well.Domain.Attribute;

    public class JobFactory : EntityFactory<JobFactory, Job>
    {
        public JobFactory()
        {
            this.Entity.Id = 1;
            this.Entity.Sequence = 1;
            this.Entity.JobTypeCode = "001";
            this.Entity.JobRef1 = "J001";
            this.Entity.JobRef1 = "J002";
            this.Entity.JobRef1 = "J0032";
            this.Entity.JobRef1 = "J004";
            this.Entity.OrderDate = DateTime.Now;
            this.Entity.Originator = "OR1";
            this.Entity.TextField1 = "Text1";
            this.Entity.TextField2 = "Text2";
            this.Entity.PerformanceStatus = PerformanceStatus.Narri;
            this.Entity.ByPassReason = ByPassReasons.Notdef;
            this.Entity.StopId = 1;
            this.Entity.EntityAttributes = new Collection<Attribute>
            {
                new Attribute
                {
                    Id = 1,
                    AttributeId =  1,
                    Code = "001",
                    Value1 = "Value1"
                }
            };

        }
    }
}
