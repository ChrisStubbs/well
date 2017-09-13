namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ICustomerRoyaltyExceptionRepository : IRepository<CustomerRoyaltyExceptionWell, int>
    {
        IEnumerable<CustomerRoyaltyExceptionWell> GetCustomerRoyaltyExceptions();

        void AddCustomerRoyaltyException(CustomerRoyaltyExceptionWell royaltyException);

        void UpdateCustomerRoyaltyException(CustomerRoyaltyExceptionWell royaltyException);

        CustomerRoyaltyExceptionWell GetCustomerRoyaltyExceptionsByRoyalty(int royalty);
    }
}