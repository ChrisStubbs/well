namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using PH.Well.Domain;

    public interface IEpodUpdateService
    {
        void Update(RouteDelivery route, string fileName);
        Job UpdateJob(JobDTO jobDto, Job existingJob, int branchId, DateTime routeDate, bool createEvents);
        IEnumerable<Job> RunPostInvoicedProcessing(List<int> updatedJobIds);
    }
}