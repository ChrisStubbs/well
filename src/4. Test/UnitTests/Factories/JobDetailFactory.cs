namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Well.Domain;

    public class JobDetailFactoryDTO : EntityFactory<JobDetailFactoryDTO, JobDetailDTO>
    {

        public JobDetailFactoryDTO()
        {
            this.Entity.LineNumber = 1;
            this.Entity.PhProductCode = "00000001";
            this.Entity.OriginalDespatchQty = 2;
            this.Entity.ProdDesc = "Product1";
            this.Entity.OrderedQty = 1;
            this.Entity.UnitMeasure = "kg";
            this.Entity.PhProductType = "Text1";
            this.Entity.PackSize = "Text2";
            this.Entity.SingleOrOuter = "Text3";
            this.Entity.SSCCBarcode = "123456789";  //tobacco bag barcode
            this.Entity.SkuGoodsValue = 2.2;
            this.Entity.DeliveredQty = 23;
            this.Entity.EntityAttributes = new List<EntityAttribute>();
        }

        public JobDetailFactoryDTO WithIsChecked(bool value)
        {
            AddEntityAttributeValue("LINESTATUS", value ? "Delivered" : "");
            return this;
        }
        public JobDetailFactoryDTO WithIsHighValue(bool value)
        {
            AddEntityAttribute("HIGHVALUE", value ? "Y" : "N");
            return this;
        }

        public JobDetailFactoryDTO AddEntityAttribute(string code, string value)
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

        public JobDetailFactoryDTO AddEntityAttributeValue(string code, string value)
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
    }

    public class JobDetailFactory : EntityFactory<JobDetailFactory, JobDetail>
    {

        public JobDetailFactory()
        {
            this.Entity.Id = 1;
            this.Entity.LineNumber = 1;
            this.Entity.PhProductCode = "00000001";
            this.Entity.OriginalDespatchQty = 2;
            this.Entity.ProdDesc = "Product1";
            this.Entity.OrderedQty = 1;
            this.Entity.UnitMeasure = "kg";
            this.Entity.PhProductType = "Text1";
            this.Entity.PackSize = "Text2";
            this.Entity.SingleOrOuter = "Text3";
            this.Entity.SSCCBarcode = "123456789";  //tobacco bag barcode
            this.Entity.SkuGoodsValue = 2.2;
            this.Entity.DeliveredQty = 23;
            this.Entity.JobId = 1;
        }
    }
}
