namespace PH.Well.UnitTests.Factories
{
    using PH.Well.Domain.ValueObjects;
    
    public class DeliveryLineFactory : EntityFactory<DeliveryLineFactory, DeliveryLine>
    {
        public DeliveryLineFactory()
        {
            this.Entity.Id = 1;
            this.Entity.JobId = 1001;
            this.Entity.LineNo = 1;
            this.Entity.ProductCode = "2001";
            this.Entity.ProductDescription = "Mars Bars";
            this.Entity.Value = 203;
            this.Entity.InvoicedQuantity = 10;
            this.Entity.ShortQuantity = 5;
            this.Entity.Reason = "It fell";
            this.Entity.Status = "Broken";
        }
    }
}
