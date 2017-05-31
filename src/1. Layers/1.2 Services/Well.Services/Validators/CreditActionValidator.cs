namespace PH.Well.Services.Validators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;


    public class CreditActionValidator : ICreditActionValidator
    {
        private readonly BulkActionResults results = new BulkActionResults();
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly IUserNameProvider userNameProvider;
        private readonly IUserRepository userRepository;

        public CreditActionValidator(ILineItemActionRepository lineItemActionRepository,
            IUserNameProvider userNameProvider,
            IUserRepository userRepository)
        {
            this.lineItemActionRepository = lineItemActionRepository;
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;

        }

        public BulkActionResults ValidateAction(BulkActionModel bulkAction)
        {
            if (bulkAction.Action == DeliveryAction.NotDefined)
            {
                results.ResultSummary = "No Action Supplied";
                results.IsActionValid = false;
                return results;
            }
            var validateUserWarning = ValidateUserForCrediting();

            if (!string.IsNullOrEmpty(validateUserWarning))
            {
                results.ResultSummary = validateUserWarning;
                results.IsActionValid = false;
                return results;
            }

            return results;
        }

        public BulkActionResults ValidateItems(BulkActionModel bulkAction)
        {
            return results; //= AddNoMatchingLineItemActionWarnings(bulkAction, lineItemActions, results);
        }

        private BulkActionResults AddNoMatchingLineItemActionWarnings(BulkActionModel bulkAction, IList<LineItemAction> lineItemActions, BulkActionResults results)
        {
            var items = bulkAction.JobDetailActionIds.Where(id => lineItemActions.All(x => x.Id != id));

            results.Items.AddRange(items.Select(id => new BulkActionResult
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
            return user.ThresholdLevelId == null ? "You must be assigned a threshold level before crediting." : null;
        }
    }
}