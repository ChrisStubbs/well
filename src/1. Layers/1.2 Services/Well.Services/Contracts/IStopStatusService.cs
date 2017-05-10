namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IStopStatusService
    {
        string DetermineStatus(List<Job> jobs);
    }
}