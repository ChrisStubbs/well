namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class LineItemActionService : ILineItemActionService
    {
        private readonly ILineItemActionRepository lineItemActionRepository;

        public LineItemActionService(ILineItemActionRepository lineItemActionRepository)
        {
            this.lineItemActionRepository = lineItemActionRepository;
        }

        public LineItemAction InsertLineItemActions(LineItemActionUpdate lineItemActionUpdate)
        {

            var lineItemAction = new LineItemAction
            {
                LineItemId = lineItemActionUpdate.LineItemId,
                DeliverAction = lineItemActionUpdate.DeliverAction,
                ExceptionType = lineItemActionUpdate.ExceptionType,
                Quantity = lineItemActionUpdate.Quantity,
                Source = lineItemActionUpdate.Source,
                Reason = lineItemActionUpdate.Reason,
            };

            lineItemActionRepository.Save(lineItemAction);
            
            return lineItemActionRepository.GetById(lineItemAction.Id);
        }

        public LineItemAction UpdateLineItemActions(LineItemActionUpdate lineItemActionUpdate)
        {
            var lineItemAction = new LineItemAction
            {
                Id = lineItemActionUpdate.Id,
                LineItemId = lineItemActionUpdate.LineItemId,
                DeliverAction = lineItemActionUpdate.DeliverAction,
                ExceptionType = lineItemActionUpdate.ExceptionType,
                Quantity = lineItemActionUpdate.Quantity,
                Source = lineItemActionUpdate.Source,
                Reason = lineItemActionUpdate.Reason,
            };

            lineItemActionRepository.Update(lineItemAction);

            return lineItemActionRepository.GetById(lineItemAction.Id);
        }
    }
}