namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public interface IEpodFileImportCommands : IImportCommands
    {
        void AfterJobCreation(Job fileJob, Job existingJob, RouteHeader routeHeader);

        void UpdateWithoutEvents(Job existingJob, int branchId, DateTime routeDate);

        IList<Job> RunPostInvoicedProcessing(IList<int> updatedJobIds);
    }
}