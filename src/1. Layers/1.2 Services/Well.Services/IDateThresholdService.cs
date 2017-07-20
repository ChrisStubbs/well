using System;
using System.Collections.Generic;
using PH.Well.Domain;

namespace PH.Well.Services
{
    public interface IDateThresholdService
    {
        DateTime EarliestSubmitDate(DateTime routeDate, int branchId);

        IList<DateThreshold> GetAll();

        void Update(DateThreshold dateThreshold);
    }
}