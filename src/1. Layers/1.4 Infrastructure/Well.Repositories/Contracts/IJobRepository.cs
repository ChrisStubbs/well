﻿namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface IJobRepository : IRepository<Job, int>
    {
        void AddJobAttributes(Attribute attribute);
        Job GetById(int id);
        void JobCreateOrUpdate(Job job);
        Job GetByAccountPicklistAndStopId(string accountId, string picklistId, int stopId);
        Job JobGetByRefDetails(string ref1, string ref2, int stopId);
        IEnumerable<CustomerRoyaltyException> GetCustomerRoyaltyExceptions();
        IEnumerable<Job> GetByStopId(int id);
        void DeleteJobById(int id, WellDeleteType deleteType);
        IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId);
        void AddCustomerRoyaltyException(CustomerRoyaltyException royaltyException);
        void UpdateCustomerRoyaltyException(CustomerRoyaltyException royaltyException);
        CustomerRoyaltyException GetCustomerRoyaltyExceptionsByRoyalty(int royalty);
    }
}