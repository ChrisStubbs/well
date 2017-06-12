namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class LineItemActionService : ILineItemActionService
    {
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly ILineItemSearchReadRepository lineItemRepository;
        private readonly ILineItemActionCommentRepository commentRepository;

        public LineItemActionService(
            ILineItemActionRepository lineItemActionRepository, 
            ILineItemSearchReadRepository lineItemRepository,
            ILineItemActionCommentRepository commentRepository)
        {
            this.lineItemActionRepository = lineItemActionRepository;
            this.lineItemRepository = lineItemRepository;
            this.commentRepository = commentRepository;
        }

        public LineItem SaveLineItemActions(int lineItemId, IEnumerable<LineItemAction> lineItemActions)
        {
           
            var lineItem = this.lineItemRepository.GetById(lineItemId);
            if (lineItem == null)
            {
                return null;
            }
            var itemActions = lineItemActions as LineItemAction[] ?? lineItemActions.ToArray();
           
            using (var transactionScope = new TransactionScope())
            {
                foreach (var action in itemActions)
                {                    
                    if (action.IsTransient())
                    {
                        action.LineItemId = lineItemId;
                        lineItemActionRepository.Save(action);
                    }
                    else
                    {
                        var original = lineItem.LineItemActions.FirstOrDefault(x => x.Id == action.Id);
                        if (original != null && original.HasChanges(action))
                        {
                            lineItemActionRepository.Update(action);
                        }
                    }

                    foreach (var comment in action.Comments.Where(x=> x.IsTransient()))
                    {
                        comment.LineItemActionId = action.Id;
                        commentRepository.Save(comment);
                    }
                }
                
                foreach (var itemToDelete in lineItem.LineItemActions.Where(x => !itemActions.Select(y => y.Id).Contains(x.Id) 
                                                                                && x.Originator != Originator.Driver))
                {
                    itemToDelete.IsDeleted = true;
                    lineItemActionRepository.Update(itemToDelete);
                }
                transactionScope.Complete();
            }

            return this.lineItemRepository.GetById(lineItemId);

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
                Originator = lineItemActionUpdate.Orginator
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
                Originator = lineItemActionUpdate.Orginator
            };

            lineItemActionRepository.Update(lineItemAction);
            return lineItemRepository.GetById(lineItemAction.LineItemId);
        }
    }
}