namespace PH.Well.UnitTests.Factories
{
    using System;
    using Well.Domain;
    using Well.Domain.Enums;

    public class LineItemActionFactory : EntityFactory<LineItemActionFactory, LineItemAction>
    {

        public LineItemActionFactory()
        {
            this.Entity.Id = 1;
            this.Entity.ExceptionType = ExceptionType.Short;
            this.Entity.Quantity = 3;
            this.Entity.Source = JobDetailSource.Delivery;
            this.Entity.Reason = JobDetailReason.Administration;
            this.Entity.ReplanDate = null;
            this.Entity.SubmittedDate = null;
            this.Entity.ApprovalDate = null;
            this.Entity.ApprovedBy = null;
            this.Entity.Originator = Originator.Driver;
            this.Entity.LineItemId = 100;
            this.Entity.ActionedBy = "TestUser";
            this.Entity.DeliveryAction = DeliveryAction.Credit;

        }
    }
}
