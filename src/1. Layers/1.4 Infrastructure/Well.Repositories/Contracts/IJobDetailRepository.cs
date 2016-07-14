namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface IJobDetailRepository : IRepository<JobDetail, int>
    {

        JobDetail GetById(int id);

        JobDetail JobDetailCreateOrUpdate(JobDetail jobDetail);

        void AddJobDetailAttributes(Attribute attribute);

        JobDetail GetByBarcodeLineNumberAndJobId(int lineNumber, string barcode, int jobId);

        void JobDetailDamageCreateOrUpdate(JobDetailDamage jobDetailDamage);
    }
}