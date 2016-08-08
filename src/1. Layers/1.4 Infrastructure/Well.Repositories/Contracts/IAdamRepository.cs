namespace PH.Well.Repositories.Contracts
{
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public interface IAdamRepository
    {
        AdamResponse CreditInvoice(CreditEvent credit, AdamSettings adamSettings);
    }
}