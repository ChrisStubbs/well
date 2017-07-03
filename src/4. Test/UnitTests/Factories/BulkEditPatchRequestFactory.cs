namespace PH.Well.UnitTests.Factories
{
    using Well.Api.Models;
    using Well.Domain.Enums;

    public class BulkEditPatchRequestFactory : EntityFactory<BulkEditPatchRequestFactory, BulkEditPatchRequest>
    {
        public BulkEditPatchRequestFactory()
        {
            Entity.DeliveryAction = DeliveryAction.Credit;
            Entity.Source = JobDetailSource.Assembler;
            Entity.Reason = JobDetailReason.AccumulatedDamages;
        }
    }
}