namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IJobDetailRepository : IRepository<JobDetail, int>
    {
        JobDetail GetById(int id);
        JobDetail GetByBarcodeLineNumberAndJobId(int lineNumber, string barcode, int jobId);
        void AddJobDetailAttributes(Attribute attribute);
        JobDetail JobDetailGetByBarcodeAndProdDesc(string barcode, int jobId);
        IEnumerable<JobDetail> GetByJobId(int id);
        JobDetail GetByJobLine(int jobId, int lineNumber);
        void DeleteJobDetailById(int id);
    }
}