namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.ObjectModel;
    using Well.Domain;

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
            this.Entity.OrdOuters = 1;
            this.Entity.InvOuters = 1;
            this.Entity.ColOuters = 1;
            this.Entity.ColBoxes = 1;
            this.Entity.ReCallPrd = false;
            this.Entity.AllowSoCrd = false;
            this.Entity.AllowSoCrd = false;
            this.Entity.Cod = false;
            this.Entity.AllowReOrd = false;
            this.Entity.SandwchOrd = false;
            this.Entity.GrnNumber = null;
            this.Entity.GrnRefusedReason = null;
            this.Entity.GrnRefusedDesc = null;
            this.Entity.ComdtyType = "1";
            this.Entity.PerformanceStatusId = 1;
            this.Entity.ByPassReasonId = 13;
            this.Entity.StopId = 1;
        }
    }
}
