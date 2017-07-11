namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain.Enums;
    using PH.Well.Domain;

    public interface IEpodUpdateService
    {
        void Update(RouteDelivery route, string fileName);
        Job UpdateJob(JobDTO jobDto, Job existingJob, int branchId, DateTime routeDate);
        void RunPostInvoicedProcessing(List<int> updatedJobIds);
    }
}