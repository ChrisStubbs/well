namespace PH.Well.UnitTests.Factories
{
    using PH.Well.Domain.ValueObjects;

    public class CreditEventFactory : EntityFactory<CreditEventFactory, CreditEvent>
    {
        public CreditEventFactory()
        {
            this.Entity.Id = 1;
            this.Entity.InvoiceNumber = "12322.1";
            this.Entity.BranchId = 22;
            this.Entity.TotalCreditValueForThreshold = 3.4M;
        }
    }
}
