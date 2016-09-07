namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using PH.Well.Common.Contracts;
    using Domain;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Repositories;
    using Well.Repositories.Contracts;

    [TestFixture]
    public class StopRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private StopRepository repository;

        private string UserName = "TestUser";

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new StopRepository(this.logger.Object, this.dapperProxy.Object);
            this.repository.CurrentUser = UserName;
        }

        public class TheGetStopByRouteHeaderId : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int routeHeaderId = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Stops_GetByRouteHeaderId")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("routeHeaderId", routeHeaderId, DbType.Int32,null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());
                var result = repository.GetStopByRouteHeaderId(1);

                dapperProxy.Verify(x=> x.WithStoredProcedure("Stops_GetByRouteHeaderId"),Times.Once);
                dapperProxy.Verify(x => x.AddParameter("routeHeaderId", routeHeaderId, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Stop>(), Times.Once());
            }
        }

        public class TheGetByIdMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Stop_GetById")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());
                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure("Stop_GetById"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Stop>(), Times.Once());
            }

        }

        public class TheSaveStopsMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldSaveRouteHeader()
            {
                var stop = StopFactory.New.Build();
                var user = UserFactory.New.Build();

                var transportOrderDetails = stop.TransportOrderRef.Split(' ');
                stop.RouteHeaderCode = transportOrderDetails[0];
                stop.DropId = transportOrderDetails[1];
                stop.LocationId = transportOrderDetails[2];
                stop.DeliveryDate = DateTime.Parse(transportOrderDetails[3]);


                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopsCreateOrUpdate))
                    .Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", stop.Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PlannedStopNumber", stop.PlannedStopNumber, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PlannedArriveTime", stop.PlannedArriveTime, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PlannedDepartTime", stop.PlannedDepartTime, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteHeaderCode", stop.RouteHeaderCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteHeaderId", stop.RouteHeaderId, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DropId", stop.DropId, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LocationId", stop.LocationId, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DeliveryDate", stop.DeliveryDate, DbType.DateTime, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SpecialInstructions", stop.SpecialInstructions, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StartWindow", stop.StartWindow, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("EndWindow", stop.EndWindow, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField1", stop.TextField1, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField2", stop.TextField2, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField3", stop.TextField3, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TextField4", stop.TextField4, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopStatusId", stop.StopStatusCodeId, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopPerformanceStatusId", stop.StopPerformanceStatusCodeId, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ByPassReasonId", stop.ByPassReasonId, DbType.Int16, null)).Returns(dapperProxy.Object);


                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopGetById))
                    .Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", stop.Id, DbType.Int32, null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());


                this.repository.StopCreateOrUpdate(stop);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopsCreateOrUpdate), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopGetById), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("PlannedStopNumber", stop.PlannedStopNumber, DbType.Int32, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<Stop>(), Times.Exactly(1));
            }
        }

        public class TheSaveStopAttributeMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldSaveStopAttributes()
            {
                var stopAttribute = StopAttributeFactory.New.Build();
                var user = UserFactory.New.Build();
                var routeHeaderId = 1;

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopAttributeCreateOrUpdate)).Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", stopAttribute.Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Code", stopAttribute.Code, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Value", stopAttribute.Value1, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", stopAttribute.AttributeId, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.AddStopAttributes(stopAttribute);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopAttributeCreateOrUpdate), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("Code", stopAttribute.Code, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }
        }

        public class TheSaveStopAccountMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldSaveStopAccountss()
            {
                var stopAccount = StopFactory.New.Build();
                var dropAndDrive = stopAccount.Accounts.IsDropAndDrive == "True";

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopAccountCreateOrUpdate)).Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", stopAccount.Accounts.Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Code", stopAccount.Accounts.Code, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AccountTypeCode", stopAccount.Accounts.AccountTypeCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DepotId", stopAccount.Accounts.DepotId, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Name", stopAccount.Accounts.Name, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Address1", stopAccount.Accounts.Address1, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Address2", stopAccount.Accounts.Address2, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PostCode", stopAccount.Accounts.PostCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactName", stopAccount.Accounts.ContactName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactNumber", stopAccount.Accounts.ContactNumber, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactNumber2", stopAccount.Accounts.ContactNumber2, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactEmailAddress", stopAccount.Accounts.ContactEmailAddress, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StartWindow", stopAccount.Accounts.StartWindow, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("EndWindow", stopAccount.Accounts.EndWindow, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Latitude", stopAccount.Accounts.Latitude, DbType.Double, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Longitude", stopAccount.Accounts.Longitude, DbType.Double, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsDropAndDrive", dropAndDrive, DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", stopAccount.Accounts.StopId, DbType.Int32, null)).Returns(dapperProxy.Object);             

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.StopAccountCreateOrUpdate(stopAccount.Accounts);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopAccountCreateOrUpdate), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("Code", stopAccount.Accounts.Code, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }
        }


        public class TheGetStopByRouteNumberAndDropNumber : StopRepositoryTests
        {
            [Test]
            public void ShouldGetStopRouteNumberAndDropNumber()
            {
                const int routeHeaderId = 1;
                const string routeHeaderCode = "001";
                const string dropId = "001";

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopGetByRouteNumberAndDropNumber)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteHeaderCode", routeHeaderCode, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteHeaderId", routeHeaderId, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DropId", dropId, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());

                var result = this.repository.GetByRouteNumberAndDropNumber(routeHeaderCode, routeHeaderId, dropId);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopGetByRouteNumberAndDropNumber), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("RouteHeaderCode", routeHeaderCode, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("RouteHeaderId", routeHeaderId, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("DropId", dropId, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Stop>(), Times.Once());
            }
        }

        public class TheDeleteStopByIdMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var deleteType = WellDeleteType.SoftDelete;
                var isSoftDelete = deleteType == WellDeleteType.SoftDelete;

                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopDeleteById))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopAttributesDeletedByStopId))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());


                this.repository.DeleteStopById(id, deleteType);


                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopDeleteById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.StopAttributesDeletedByStopId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("StopId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("StopId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());
            }
        }

    }
}
