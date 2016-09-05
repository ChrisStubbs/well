namespace PH.Well.UnitTests.Factories
{
    using Well.Domain;
    using System.Collections.ObjectModel;
    using Well.Domain.Enums;
    using Attribute = Well.Domain.Attribute;

    public class JobDetailFactory : EntityFactory<JobDetailFactory, JobDetail>
    {

        public JobDetailFactory()
        {
            this.Entity.Id = 1;
            this.Entity.LineNumber = 1;
            this.Entity.BarCode = "00000001";
            this.Entity.OriginalDespatchQty = 2;
            this.Entity.ProdDesc = "Product1";
            this.Entity.OrderedQty = 1;
            this.Entity.SkuWeight = 1.1m;
            this.Entity.SkuCube = 1.5m;
            this.Entity.UnitMeasure = "kg";
            this.Entity.TextField1 = "Text1";
            this.Entity.TextField2 = "Text2";
            this.Entity.TextField3 = "Text3";
            this.Entity.TextField4 = "Text4";
            this.Entity.TextField5 = "Text5";
            this.Entity.SkuGoodsValue = 2.2;
            this.Entity.JobId = 1;
            this.Entity.JobDetailDamages = new Collection<JobDetailDamage>
            {
                new JobDetailDamage
                {
                    Id = 1,
                    Qty = 1m,
                    DamageReason = DamageReasons.CAR01

                }
            };
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
