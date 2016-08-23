namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IJobDetailRepository : IRepository<JobDetail, int>
    {

        JobDetail GetById(int id);

        JobDetail GetByJobLine(int jobId, int lineNumber);

        JobDetail GetByBarcodeLineNumberAndJobId(int lineNumber, string barcode, int jobId);

        void AddJobDetailAttributes(Attribute attribute);


        //JobDetail GetByBarcodeAndProdDesc(string barcode, int jobId);

        JobDetail JobDetailGetByBarcodeAndProdDesc(string barcode, int jobId);

        JobDetail JobDetailCreateOrUpdate(JobDetail jobDetail);

        void CreateOrUpdateJobDetailDamage(JobDetailDamage jobDetailDamage);

        IEnumerable<JobDetail> GetByJobId(int id);

        void DeleteJobDetailById(int id);

    }
}