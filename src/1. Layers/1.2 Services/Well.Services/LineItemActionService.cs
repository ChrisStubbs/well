namespace PH.Well.Services
{
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class LineItemActionService : ILineItemActionService
    {
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly ILineItemSearchReadRepository lineItemRepository;

        public LineItemActionService(ILineItemActionRepository lineItemActionRepository, ILineItemSearchReadRepository lineItemRepository)
        {
            this.lineItemActionRepository = lineItemActionRepository;
            this.lineItemRepository = lineItemRepository;
        }

        public LineItem InsertLineItemActions(LineItemActionUpdate lineItemActionUpdate)
        {

            var lineItemAction = new LineItemAction
            {
                LineItemId = lineItemActionUpdate.LineItemId,
                DeliveryAction = lineItemActionUpdate.DeliverAction,
                ExceptionType = lineItemActionUpdate.ExceptionType,
                Quantity = lineItemActionUpdate.Quantity,
                Source = lineItemActionUpdate.Source,
                Reason = lineItemActionUpdate.Reason,
                Originator = Originator.Customer
            };

            lineItemActionRepository.Save(lineItemAction);

            return lineItemRepository.GetById(lineItemAction.LineItemId);
        }

        public LineItem UpdateLineItemActions(LineItemActionUpdate lineItemActionUpdate)
        {
            var lineItemAction = new LineItemAction
            {
                Id = lineItemActionUpdate.Id,
                LineItemId = lineItemActionUpdate.LineItemId,
                DeliveryAction = lineItemActionUpdate.DeliverAction,
                ExceptionType = lineItemActionUpdate.ExceptionType,
                Quantity = lineItemActionUpdate.Quantity,
                Source = lineItemActionUpdate.Source,
                Reason = lineItemActionUpdate.Reason,
            };

            lineItemActionRepository.Update(lineItemAction);
            return lineItemRepository.GetById(lineItemAction.LineItemId);
        }
    }
}