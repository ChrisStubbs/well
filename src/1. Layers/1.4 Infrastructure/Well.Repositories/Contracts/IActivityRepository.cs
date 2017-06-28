namespace PH.Well.Repositories.Contracts
{
    using Domain.ValueObjects;

    public interface IActivityRepository
    {
        ActivitySource GetActivitySourceByDocumentNumber(string documentNumber, int branchId);
    }
}
