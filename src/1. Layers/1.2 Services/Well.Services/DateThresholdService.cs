namespace PH.Well.Services
{
    using System;

    public class DateThresholdService : IDateThresholdService
    {
        //TODO: Add logic for branch to calculate Earliest credit date
        public DateTime EarliestCreditDate(DateTime routeDate, int branchId)
        {
            return routeDate.AddDays(2);
        }
    }
}