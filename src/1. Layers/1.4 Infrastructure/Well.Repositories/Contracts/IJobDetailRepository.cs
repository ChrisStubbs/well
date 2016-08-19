namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface IJobDetailRepository : IRepository<JobDetail, int>
    {

        JobDetail GetById(int id);

        JobDetail GetByJobLine(int jobId, int lineNumber);

        void CreateOrUpdate(JobDetail jobDetail);

        void CreateOrUpdateJobDetailAttributes(Attribute attribute);

        JobDetail GetByBarcodeLineNumberAndJobId(int lineNumber, string barcode, int jobId);

        void CreateOrUpdateJobDetailDamage(JobDetailDamage jobDetailDamage);

        JobDetail GetByBarcodeAndProdDesc(string barcode, int jobId);
    }
}