namespace PH.Well.UnitTests.Factories
{
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    
    public class DeliveryDetailFactory : EntityFactory<DeliveryDetailFactory, DeliveryDetail>
    {
        public DeliveryDetailFactory()
        {
            this.Entity.Id = 1;
            this.Entity.AccountCode = "123.32122";
            this.Entity.Status = PerformanceStatus.Compl;
            this.Entity.AccountName = "Mars Industries";
            this.Entity.AccountAddress = "123 Mars Road";
            this.Entity.InvoiceNumber = "54333";
            this.Entity.ContactName = "Mr Mars";
            this.Entity.PhoneNumber = "01444 222333";
            this.Entity.MobileNumber = "0780 143222";
            this.Entity.DeliveryType = "Uplift";
            this.Entity.GrnNumber = "231211221";
        }
    }
}
