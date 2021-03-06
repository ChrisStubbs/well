﻿namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public interface IManualCompletionService
    {
        IEnumerable<Job> Complete(IEnumerable<int> jobIds, ManualCompletionType type);
        IEnumerable<Job> GetJobsAvailableForCompletion(IEnumerable<int> jobIds);
        bool CanManuallyComplete(Job job);
    }
}