namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.ObjectModel;
    using Well.Domain;

    public class StopFactory : EntityFactory<StopFactory, Stop>
    {
        public StopFactory()
        {
            this.Entity.Id = 1;
            this.Entity.PlannedStopNumber = "001";
            this.Entity.TransportOrderRef = "0001 01 LOC001 " + DateTime.Now.ToString("dd-MM-yyyy");
            this.Entity.RouteHeaderId = 1;
            this.Entity.RouteHeaderCode = "0001";
            this.Entity.DropId = "01";
            this.Entity.LocationId = "LOC001";
            this.Entity.DeliveryDate = DateTime.Now;
            this.Entity.ShellActionIndicator = "N";
            this.Entity.CustomerShopReference = "CustRef1";
            this.Entity.StopStatusCodeId = 2;
            this.Entity.StopPerformanceStatusCodeId = 6;
            this.Entity.ByPassReasonId = 13;
            this.Entity.Jobs = new Collection<Job>();

            this.Entity.Account = new Account
            {
                Id = 1,
                Code = "AC001",
                AccountTypeCode = "AT001",
                DepotId = 1,
                Name = "ACCOUNT001",
                Address1 = "Davingdor Road",
                Address2 = "Hove",
                PostCode = "BN1 1RE",
                ContactName = "Andrew Smith",
                ContactNumber = "01293 100 000",
                ContactNumber2 = string.Empty,
                ContactEmailAddress = "A.Smith@palmerharvey.co.uk",
                IsDeleted = false,
                StopId = this.Entity.Id
            };

            this.Entity.EntityAttributes = new Collection<EntityAttribute>();
            this.Entity.EntityAttributes.Add(new EntityAttribute {Code = "ActualPaymentCash", Value = null});
            this.Entity.EntityAttributes.Add(new EntityAttribute {Code = "ActualPaymentCheque", Value = null});
            this.Entity.EntityAttributes.Add(new EntityAttribute {Code = "ActualPaymentCard", Value = null});
        }
    }
}