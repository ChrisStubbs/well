namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using Enums;

    public class SubmitActionModel
    {
        public DeliveryAction Action { get; set; }
        public int[] JobIds { get; set; }

        public IEnumerable<LineItemActionSubmitModel> ItemsToSubmit { get; private set; }

        public void SetItemsToSubmit(IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems)
        {
            if (ItemsToSubmit == null)
            {
                ItemsToSubmit = LineItemActionSubmitModel.GetItemsContainingJobIds(allUnsubmittedItems.ToList(), JobIds);
            }
        }
    }
}