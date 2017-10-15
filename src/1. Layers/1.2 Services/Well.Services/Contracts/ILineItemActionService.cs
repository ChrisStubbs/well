using PH.Well.Domain.Enums;

namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface ILineItemActionService
    {
        LineItem InsertLineItemActions(LineItemActionUpdate lineItemActionUpdate);
        LineItem UpdateLineItemActions(LineItemActionUpdate lineItemActionUpdate);
        LineItem SaveLineItemActions(Job job, int lineItemId, IEnumerable<LineItemAction> lineItemActions);

        /// <summary>
        /// Check whether delivery acton can be used for specific job
        /// </summary>
        /// <param name="job"></param>
        /// <param name="deliveryAction"></param>
        /// <returns></returns>
        bool CanSetActionForJob(Job job,  DeliveryAction deliveryAction);

        void CloseExceptionsForBranch(int branchId, DateTime routeDate);
    }
}