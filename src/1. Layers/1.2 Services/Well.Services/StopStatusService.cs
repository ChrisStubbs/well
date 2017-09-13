namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;

    public class StopStatusService : IStopStatusService
    {
        public const string StatusException = "Exception";
        public const string StatusIncomplete = "Incomplete";
        public const string StatusClean = "Clean";
        public const string StatusUndetermined = "Undetermined";

        //Status Should be one of the below
        //Planned -- No invoice No
        //Invoiced -- where some are incomplete
        //Complete
        //Bypassed -- All Jobs Bypassed

        public string DetermineStatus(IList<Job> jobs)
        {
          
            if (!jobs.Any())
            {
                return StatusIncomplete;
            }

            if (jobs.Any(x => x.JobStatus == JobStatus.AwaitingInvoice || x.JobStatus == JobStatus.InComplete))
            {
                return StatusIncomplete;
            }

            if (jobs.Any(x => x.JobStatus == JobStatus.Exception || x.JobStatus == JobStatus.Bypassed))
            {
                return StatusException;
            }

            if (jobs.All(x => x.JobStatus == JobStatus.Clean || x.JobStatus == JobStatus.Resolved || x.JobStatus == JobStatus.DocumentDelivery || x.JobStatus == JobStatus.CompletedOnPaper))
            {
                return StatusClean;
            }

            return StatusUndetermined;
        }
    }
}