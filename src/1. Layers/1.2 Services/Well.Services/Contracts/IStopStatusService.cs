namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IStopStatusService
    {
        string DetermineStatus(IList<Job> jobs);
    }
}