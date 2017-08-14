namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ICustomerRoyaltyExceptionRepository : IRepository<CustomerRoyaltyException, int>
    {
        IEnumerable<CustomerRoyaltyException> GetCustomerRoyaltyExceptions();

        void AddCustomerRoyaltyException(CustomerRoyaltyException royaltyException);

        void UpdateCustomerRoyaltyException(CustomerRoyaltyException royaltyException);

        CustomerRoyaltyException GetCustomerRoyaltyExceptionsByRoyalty(int royalty);
    }
}