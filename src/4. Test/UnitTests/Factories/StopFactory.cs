namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.ObjectModel;
    using Well.Domain;
    using Attribute = Well.Domain.Attribute;

    public class StopFactory : EntityFactory<StopFactory,Stop>
    {
        public StopFactory()
        {
            this.Entity.Id = 1;
            this.Entity.PlannedStopNumber = "001";
            this.Entity.PlannedArriveTime = "09:00";
            this.Entity.PlannedDepartTime = "11:00";
            this.Entity.RouteHeaderId = 1;
            this.Entity.RouteHeaderCode = "0001";
            this.Entity.DropId = "01";
            this.Entity.LocationId = "LOC001";
            this.Entity.DeliveryDate = DateTime.Now;
            this.Entity.SpecialInstructions = "Keep Chilled";
            this.Entity.StartWindow = "09:00";
            this.Entity.EndWindow = "11:00";
            this.Entity.TextField1 = "AC001";
            this.Entity.TextField2 = "STP001";
            this.Entity.TextField3 = string.Empty;
            this.Entity.TextField4 = string.Empty;
            this.Entity.StopStatusCodeId = 2;
            this.Entity.StopPerformanceStatusCodeId = 6;
            this.Entity.ByPassReasonId = 13;
            this.Entity.Jobs = new Collection<Job>();

            this.Entity.Accounts = new Account
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
                StartWindow = "09:00",
                EndWindow = "11:00",
                Longitude = 120,
                Latitude = 130,
                IsDropAndDrive = "True",
                StopId = this.Entity.Id
            };  

            this.Entity.EntityAttributes = new Collection<Attribute>();
        }
    }
}
