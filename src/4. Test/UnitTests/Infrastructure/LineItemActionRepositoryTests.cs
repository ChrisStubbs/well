namespace PH.Well.UnitTests.Infrastructure
{
    using System.Data;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Common.Contracts;

    [TestFixture]
    public class LineItemActionRepositoryTests
    {
        private Mock<ILogger> logger;
        private Mock<IDapperProxy> dapperProxy;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IDbConfiguration> dbConfig;

        private LineItemActionRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dbConfig = new Mock<IDbConfiguration>();
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);
            dapperProxy.Setup(x => x.DbConfiguration).Returns(dbConfig.Object);

            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("TestUser");

            this.repository = new LineItemActionRepository(this.dapperProxy.Object, this.logger.Object, this.userNameProvider.Object);

        }

        public class TheSaveLineItemActionMethod : LineItemActionRepositoryTests
        {
            [Test]
            public void ShouldSaveLineItemAction()
            {
                var lineItemAction = LineItemActionFactory.New.Build();
                var storedProcedure = "LineItemAction_InsertByUser";

                dapperProxy.Setup(x => x.WithStoredProcedure(storedProcedure)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ExceptionTypeId", lineItemAction.ExceptionType, DbType.Int32, null))
                  .Returns(dapperProxy.Object);
           
                dapperProxy.Setup(x => x.AddParameter("Quantity", lineItemAction.Quantity, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SourceId", lineItemAction.Source, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ReasonId", lineItemAction.Reason, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ReplanDate", lineItemAction.ReplanDate, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SubmittedDate", lineItemAction.SubmittedDate, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ApprovalDate", lineItemAction.ApprovalDate, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ApprovedBy", lineItemAction.ApprovedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LineItemId", lineItemAction.LineItemId, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Originator", lineItemAction.Originator, DbType.Int16, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ActionedBy", lineItemAction.ActionedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DeliveryActionId", lineItemAction.DeliveryAction, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("CreatedDate", lineItemAction.DateCreated, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DbType>(), null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.Save(lineItemAction);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(storedProcedure), Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("ExceptionTypeId", lineItemAction.ExceptionType, DbType.Int32, null),
                 Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("Quantity", lineItemAction.Quantity, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SourceId", lineItemAction.Source, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ReasonId", lineItemAction.Reason, DbType.Int32, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ReplanDate", lineItemAction.ReplanDate, DbType.DateTime, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SubmittedDate", lineItemAction.SubmittedDate, DbType.DateTime, null),
                      Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ApprovalDate", lineItemAction.ApprovalDate, DbType.DateTime, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ApprovedBy", lineItemAction.ApprovedBy, DbType.String, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("LineItemId", lineItemAction.LineItemId, DbType.Int32, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Originator", lineItemAction.Originator, DbType.Int16, null),
                      Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ActionedBy", lineItemAction.ActionedBy, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DeliveryActionId", lineItemAction.DeliveryAction, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("CreatedDate", lineItemAction.DateCreated, DbType.DateTime, null),
                    Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }

            [Test]
            public void ShouldUpdateExistingLineItemAction()
            {
                var lineItemAction = LineItemActionFactory.New.Build();
                var storedProcedure = "LineItemActionUpdate";

                dapperProxy.Setup(x => x.WithStoredProcedure(storedProcedure)).Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Id", lineItemAction.Id, DbType.Int32, null))
                 .Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("ExceptionTypeId", lineItemAction.ExceptionType, DbType.Int32, null))
                  .Returns(dapperProxy.Object);

                dapperProxy.Setup(x => x.AddParameter("Quantity", lineItemAction.Quantity, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SourceId", lineItemAction.Source, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ReasonId", lineItemAction.Reason, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ReplanDate", lineItemAction.ReplanDate, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("SubmittedDate", lineItemAction.SubmittedDate, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ApprovalDate", lineItemAction.ApprovalDate, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ApprovedBy", lineItemAction.ApprovedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("LineItemId", lineItemAction.LineItemId, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Originator", lineItemAction.Originator, DbType.Int16, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("ActionedBy", lineItemAction.ActionedBy, DbType.String, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("DeliveryActionId", lineItemAction.DeliveryAction, DbType.Int32, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UpdatedDate", lineItemAction.DateUpdated, DbType.DateTime, null))
                    .Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DbType>(), null))
                    .Returns(dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.Update(lineItemAction);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(storedProcedure), Times.Exactly(1));


                dapperProxy.Verify(x => x.AddParameter("Id", lineItemAction.Id, DbType.Int32, null),
               Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("ExceptionTypeId", lineItemAction.ExceptionType, DbType.Int32, null),
                 Times.Exactly(1));

                dapperProxy.Verify(x => x.AddParameter("Quantity", lineItemAction.Quantity, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SourceId", lineItemAction.Source, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ReasonId", lineItemAction.Reason, DbType.Int32, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ReplanDate", lineItemAction.ReplanDate, DbType.DateTime, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("SubmittedDate", lineItemAction.SubmittedDate, DbType.DateTime, null),
                      Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ApprovalDate", lineItemAction.ApprovalDate, DbType.DateTime, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ApprovedBy", lineItemAction.ApprovedBy, DbType.String, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("LineItemId", lineItemAction.LineItemId, DbType.Int32, null),
                     Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("Originator", lineItemAction.Originator, DbType.Int16, null),
                      Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("ActionedBy", lineItemAction.ActionedBy, DbType.String, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("DeliveryActionId", lineItemAction.DeliveryAction, DbType.Int32, null),
                    Times.Exactly(1));
                dapperProxy.Verify(x => x.AddParameter("UpdatedDate", lineItemAction.DateUpdated, DbType.DateTime, null),
                    Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Execute(), Times.Exactly(1));

            }
        }
    }
}
