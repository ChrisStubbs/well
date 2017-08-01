﻿namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class CustomerRoyaltyExceptionRepository : DapperRepository<CustomerRoyaltyException, int>, ICustomerRoyaltyExceptionRepository
    {
        public CustomerRoyaltyExceptionRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        public IEnumerable<CustomerRoyaltyException> GetCustomerRoyaltyExceptions()
        {
            var customerRoyaltyException =
                dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyalExceptionGet)
                    .Query<CustomerRoyaltyException>();

            return customerRoyaltyException;
        }

        public CustomerRoyaltyException GetCustomerRoyaltyExceptionsByRoyalty(int royalty)
        {
            var customerRoyaltyException =
                dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyalExceptionGetByRoyalty)
                    .AddParameter("RoyaltyCode", royalty, DbType.Int32)
                    .Query<CustomerRoyaltyException>();

            return customerRoyaltyException.FirstOrDefault();
        }

        public void AddCustomerRoyaltyException(CustomerRoyaltyException royaltyException)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyaltyExceptionInsert)
                .AddParameter("RoyaltyCode", royaltyException.RoyaltyCode, DbType.Int32)
                .AddParameter("Customer", royaltyException.Customer, DbType.String)
                .AddParameter("ExceptionDays", royaltyException.ExceptionDays, DbType.Byte)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
        }

        public void UpdateCustomerRoyaltyException(CustomerRoyaltyException royaltyException)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyaltyExceptionUpdate)
                .AddParameter("Id", royaltyException.Id, DbType.Int32)
                .AddParameter("RoyaltyCode", royaltyException.RoyaltyCode, DbType.Int32)
                .AddParameter("Customer", royaltyException.Customer, DbType.String)
                .AddParameter("ExceptionDays", royaltyException.ExceptionDays, DbType.Byte)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
        }
    }
}