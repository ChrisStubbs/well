namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using Domain;
    using Domain.Enums;

    public interface IJobRepository : IRepository<Job, int>
    {
        void AddJobAttributes(Attribute attribute);
        Job GetById(int id);
        Job JobCreateOrUpdate(Job job);
        Job GetByAccountPicklistAndStopId(string accountId, string picklistId, int stopId);
    }
}