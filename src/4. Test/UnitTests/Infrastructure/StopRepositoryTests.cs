namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
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

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopsCreateOrUpdate))
                    .Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", stop.Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TransportOrderReference", stop.TransportOrderReference, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PlannedStopNumber", stop.PlannedStopNumber, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteHeaderCode", stop.RouteHeaderCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteHeaderId", stop.RouteHeaderId, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DropId", stop.DropId, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LocationId", stop.LocationId, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DeliveryDate", stop.DeliveryDate, DbType.DateTime, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ShellActionIndicator", stop.ShellActionIndicator, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("CustomerShopReference", stop.CustomerShopReference, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AllowOvers", stop.AllowOvers == "True", DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("CustUnatt", stop.CustUnatt == "True", DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PHUnatt", stop.PHUnatt == "True", DbType.Boolean, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopStatusId", stop.StopStatusCodeId, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopPerformanceStatusId", stop.StopPerformanceStatusCodeId, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ByPassReasonId", stop.ByPassReasonId, DbType.Int16, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ActualPaymentCash", stop.ActualPaymentCash, DbType.Decimal , null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ActualPaymentCheque", stop.ActualPaymentCheque, DbType.Decimal, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ActualPaymentCard", stop.ActualPaymentCard, DbType.Decimal, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AccountBalance", stop.AccountBalance, DbType.Decimal, null)).Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.StopCreateOrUpdate(stop);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopsCreateOrUpdate), Times.Exactly(1));
                this.dapperProxy.Verify(x => x.AddParameter("PlannedStopNumber", stop.PlannedStopNumber, DbType.Int32, null), Times.Exactly(1));
                this.dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));
                this.dapperProxy.Verify(x => x.AddParameter("TransportOrderReference", stop.TransportOrderReference, DbType.String, null), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("PlannedStopNumber", stop.PlannedStopNumber, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("RouteHeaderCode", stop.RouteHeaderCode, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("RouteHeaderId", stop.RouteHeaderId, DbType.Int32, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DropId", stop.DropId, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("LocationId", stop.LocationId, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DeliveryDate", stop.DeliveryDate, DbType.DateTime, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ShellActionIndicator", stop.ShellActionIndicator, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("CustomerShopReference", stop.CustomerShopReference, DbType.String, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("AllowOvers", stop.AllowOvers == "True", DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("CustUnatt", stop.CustUnatt == "True", DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("PHUnatt", stop.PHUnatt == "True", DbType.Boolean, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("StopStatusId", stop.StopStatusCodeId, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("StopPerformanceStatusId", stop.StopPerformanceStatusCodeId, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ByPassReasonId", stop.ByPassReasonId, DbType.Int16, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ActualPaymentCash", stop.ActualPaymentCash, DbType.Decimal, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ActualPaymentCheque", stop.ActualPaymentCheque, DbType.Decimal, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ActualPaymentCard", stop.ActualPaymentCard, DbType.Decimal, null), Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("AccountBalance", stop.AccountBalance, DbType.Decimal, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));
            }
        }

        public class TheSaveStopAccountMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldSaveStopAccounts()
            {
                var stopAccount = StopFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopAccountCreateOrUpdate)).Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", stopAccount.Account.Id, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Code", stopAccount.Account.Code, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("AccountTypeCode", stopAccount.Account.AccountTypeCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DepotId", stopAccount.Account.DepotId, DbType.Int32, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Name", stopAccount.Account.Name, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Address1", stopAccount.Account.Address1, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Address2", stopAccount.Account.Address2, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PostCode", stopAccount.Account.PostCode, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactName", stopAccount.Account.ContactName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactNumber", stopAccount.Account.ContactNumber, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactNumber2", stopAccount.Account.ContactNumber2, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ContactEmailAddress", stopAccount.Account.ContactEmailAddress, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", stopAccount.Account.StopId, DbType.Int32, null)).Returns(dapperProxy.Object);             

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.StopAccountCreateOrUpdate(stopAccount.Account);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopAccountCreateOrUpdate), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("Code", stopAccount.Account.Code, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }
        }


        public class TheGetStopByRouteNumberAndDropNumber : StopRepositoryTests
        {
            [Test]
            public void ShouldGetStopRouteNumberAndDropNumber()
            {
                var transportOrderReference = "BRI-999911111";

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopGetByTransportOrderReference)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TransportOrderReference", transportOrderReference, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());

                var result = this.repository.GetByTransportOrderReference(transportOrderReference);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopGetByTransportOrderReference), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("TransportOrderReference", transportOrderReference, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Stop>(), Times.Once());
            }
        }

        public class TheDeleteStopByIdMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopDeleteById))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                this.repository.DeleteStopById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopDeleteById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("StopId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());
            }
        }

    }
}
