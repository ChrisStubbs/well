namespace PH.Well.UnitTests.Factories
{
    using System;
    using PH.Well.Domain.ValueObjects;
    
    public class DeliveryFactory : EntityFactory<DeliveryFactory, Delivery>
    {
        public DeliveryFactory()
        {
            this.Entity.Id = 1;
            this.Entity.RouteNumber = "011";
            this.Entity.DropId = 1;
            this.Entity.AccountName = "Mars Industries";
            this.Entity.InvoiceNumber = "54333";
            this.Entity.JobStatus = "On time";
            this.Entity.DeliveryDate = DateTime.Now;
            this.Entity.Reason = "";
            this.Entity.Action = "";
            this.Entity.Assigned = "Jill Prior";
            this.Entity.AccountId = "123.22111";
            this.Entity.BranchId = 21;
        }
    }
}
