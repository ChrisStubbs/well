namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly IUserRepository userRepository;

        public BulkActionService(ILineItemActionRepository lineItemActionRepository,
                    IUserNameProvider userNameProvider,
                    IUserRepository userRepository)
        {
            this.lineItemActionRepository = lineItemActionRepository;
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
        }

        public BulkActionResults ApplyAction(BulkActionModel bulkAction)
        {
            var results = new BulkActionResults();

            if (bulkAction.Action == DeliveryAction.NotDefined)
            {
                results.ResultSummary = "No Action Supplied";
                return results;
            }

            var validateUserWarning = ValidateUserForCrediting();
            if (!string.IsNullOrEmpty(validateUserWarning))
            {
                results.ResultSummary = validateUserWarning;
                return results;
            }

            var lineItemActions = lineItemActionRepository.GetByIds(bulkAction.JobDetailActionIds);
            results = AddNoMatchingLineItemActionWarnings(bulkAction, lineItemActions, results);

            results = ValidateActionItems(bulkAction, lineItemActions, results);
            //TODO: Need top complete validation and do the save

            return results;
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

        private BulkActionResults AddNoMatchingLineItemActionWarnings(BulkActionModel bulkAction, IList<LineItemAction> lineItemActions, BulkActionResults results)
        {
            var items = bulkAction.JobDetailActionIds.Where(id => lineItemActions.All(x => x.Id != id));

            results.Results.AddRange(items.Select(id => new BulkActionResult
            {
                JobDetailActionId = id,
                Message = $"JobDetailActionId {id} does not exist",
                Type = BulkActionResultType.Error,
                CanAction = false
            }));

            return results;
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