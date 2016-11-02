namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Data;
    using Domain;

    public interface IJobDetailRepository : IRepository<JobDetail, int>
    {
        JobDetail GetById(int id);

        IEnumerable<JobDetail> GetByJobId(int jobId);

        JobDetail GetByJobLine(int jobId, int lineNumber);

        void DeleteJobDetailById(int id);

        void CreditLines(DataTable creditLinesTable);
    }
}