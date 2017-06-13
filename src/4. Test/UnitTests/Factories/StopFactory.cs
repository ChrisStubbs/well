namespace PH.Well.UnitTests.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Well.Domain;

    public class StopFactoryDTO : EntityFactory<StopFactoryDTO, StopDTO>
    {
        public StopFactoryDTO()
        {
            this.Entity.Id = 1;
            this.Entity.PlannedStopNumber = "001";
            this.Entity.TransportOrderReference = "BIR-000000000000001";
            this.Entity.RouteHeaderId = 1;
            this.Entity.RouteHeaderCode = "0001";
            this.Entity.DropId = "01";
            this.Entity.LocationId = "LOC001";
            this.Entity.DeliveryDate = DateTime.Now;
            this.Entity.ShellActionIndicator = "N";
            this.Entity.StopStatusCode = "a status";
            this.Entity.StopStatusDescription = "something";
            this.Entity.StopByPassReason = "Some reason";
            this.Entity.Jobs = new List<JobDTO>();
            this.Entity.WellStatusId = 3;

            this.Entity.Account = new AccountDTO
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

            this.Entity.EntityAttributes = new List<EntityAttribute>
            {
                new EntityAttribute { Code = "ActualPaymentCash", Value = null },
                new EntityAttribute { Code = "ActualPaymentCheque", Value = null },
                new EntityAttribute { Code = "ActualPaymentCard", Value = null }
            };
        }

        public StopFactoryDTO AddEntityAttribute(string code, string value)
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
    }

    public class StopFactory : EntityFactory<StopFactory, Stop>
    {
        public StopFactory()
        {
            this.Entity.Id = 1;
            this.Entity.PlannedStopNumber = "001";
            this.Entity.TransportOrderReference = "BIR-000000000000001";
            this.Entity.RouteHeaderId = 1;
            this.Entity.RouteHeaderCode = "202000";
            this.Entity.DropId = "01";
            this.Entity.LocationId = "LOC001";
            this.Entity.DeliveryDate = DateTime.Now;
            this.Entity.ShellActionIndicator = "N";
            this.Entity.StopStatusCode = "a status";
            this.Entity.StopStatusDescription = "something";
            this.Entity.StopByPassReason = "Some reason";
            this.Entity.Jobs = new List<Job>();
            this.Entity.WellStatusId = 3;

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
        }
    }
}