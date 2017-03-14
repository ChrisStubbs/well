﻿using PH.Well.Domain;

namespace PH.Well.Services.Contracts
{
    public interface IJobStatusService
    {
        void SetInitialStatus(Job job);

        void SetIncompleteStatus(Job job);

        Job DetermineStatus(Job job, int branchId);
    }
}
