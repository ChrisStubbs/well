namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public interface IJobDetailRepository : IRepository<JobDetail, int>
    {
        JobDetail GetById(int id);
        IEnumerable<JobDetail> GetByJobId(int jobId);
        JobDetail GetByJobLine(int jobId, int lineNumber);
        void AddJobDetailAttributes(Attribute attribute);
        void DeleteJobDetailById(int id, WellDeleteType deleteType);
    }
}