namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class BulkActionService : IBulkActionService
    {
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly IUserNameProvider userNameProvider;
        private readonly ICreditActionValidator creditActionValidator;
        private readonly IUserRepository userRepository;

        public BulkActionService(ILineItemActionRepository lineItemActionRepository,
                    IUserNameProvider userNameProvider,
                    ICreditActionValidator creditActionValidator)
        {
            this.lineItemActionRepository = lineItemActionRepository;
            this.userNameProvider = userNameProvider;
            this.creditActionValidator = creditActionValidator;
        }

        public BulkActionResults ApplyAction(BulkActionModel bulkAction)
        {
            var results = new BulkActionResults();
            var validator = GetValidator(bulkAction.Action);

            if (validator != null)
            {
                results = validator.ValidateAction(bulkAction);

                if (results.IsActionValid)
                {
                    results = validator.ValidateItems(bulkAction);

                    var canActionResults = results.Items.Where(x => x.CanAction).ToArray();

                    if (canActionResults.Any())
                    {
                        var itemsToAction = lineItemActionRepository.GetByIds(canActionResults.Select(x => x.JobDetailActionId).ToArray());

                        using (var transactionScope = new TransactionScope())
                        {
                            foreach (var item in itemsToAction)
                            {
                                item.DeliveryAction = bulkAction.Action;
                                lineItemActionRepository.Update(item);
                                //TODO: How and when do we resolve a Job?
                            }
                            transactionScope.Complete();
                        }
                    }
                    
                    //results = AddNoMatchingLineItemActionWarnings(bulkAction, lineItemActions, results);
                    //results = ValidateActionItems(bulkAction, lineItemActions, results);
                }
                
                return results;
            }

            results.ResultSummary = $"No Validator found for Action {bulkAction.Action} vour action has not been completed";
            results.IsActionValid = false;

            return results;

        }

        public IActionValidator GetValidator(DeliveryAction deliveryAction)
        {
            //TODO: Move this to its own factory class
            switch (deliveryAction)
            {
                case DeliveryAction.NotDefined:
                    break;
                case DeliveryAction.Credit:
                    return creditActionValidator;
                case DeliveryAction.MarkAsBypassed:
                    break;
                case DeliveryAction.MarkAsDelivered:
                    break;
                default:
                    break;
            }
            return null;
        }



        private BulkActionResults ValidateActionItems(BulkActionModel bulkAction, IList<LineItemAction> lineItemActions, BulkActionResults results)
        {
            foreach (var lineItemAction in lineItemActions)
            {
                var resultsItem = new BulkActionResult { JobDetailActionId = lineItemAction.Id };

                switch (bulkAction.Action)
                {
                    case DeliveryAction.NotDefined:
                        break;
                    case DeliveryAction.Credit:
                        if (HasQuantity(lineItemAction, resultsItem))
                        {
                            //TODO: Continue from here
                        }

                        break;
                    case DeliveryAction.MarkAsDelivered:
                        break;
                    case DeliveryAction.MarkAsBypassed:
                        break;
                }
            }

            return results;
        }

        private bool HasQuantity(LineItemAction lineItemAction, BulkActionResult resultsItem)
        {
            if (lineItemAction.Quantity <= 0)
            {
                resultsItem.CanAction = false;
                resultsItem.Message = $"Can not credit item {lineItemAction.Id} does not exist";
                resultsItem.Type = BulkActionResultType.Error;
                resultsItem.CanAction = false;

            }
            return false;
        }

        

        private string ValidateUserForCrediting()
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                return $"User not found ({username})";
            }

            if (user.ThresholdLevelId == null)
            {
                return $"You must be assigned a threshold level before crediting.";
            }

            return string.Empty;
        }
    }
}