﻿namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.Generic;

    using Well.Domain;
    using System.Collections.ObjectModel;
    using Well.Domain.Enums;

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
            this.Entity.TobaccoBagBarcode = "123456789";
            this.Entity.SkuGoodsValue = 2.2;
            this.Entity.DeliveredQty = 23;
            this.Entity.JobId = 1;
            this.Entity.EntityAttributes = new List<EntityAttribute>();
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "SubOuterDamageTotal", Value = "100" });
            this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "IsHighValue", Value = "N" });
            //this.Entity.EntityAttributes.Add(new EntityAttribute { Code = "DateLife", Value = DateTime.Now.ToShortDateString()});

        }
    }
}
