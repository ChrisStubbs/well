namespace PH.Well.Services.Contracts
{
    using System;
    using Domain;

    public interface IPodService
    {
        /// <summary>
        /// Create Pod event 
        /// </summary>
        void CreatePodEvent(Job job, int branchId, DateTime routeDate);
    }
}
