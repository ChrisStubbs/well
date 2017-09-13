namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Enums;

    public class SubmitActionResult
    {
        public SubmitActionResult()
        {
            Warnings = new List<string>();
            Details = new List<SubmitActionResultDetails>();
        }

        public string Message { get; set; }
        public bool IsValid { get; set; }
        public IList<string> Warnings { get; set; }
        public IList<SubmitActionResultDetails> Details { get; set; }
    }

    public class SubmitActionResultDetails
    {
        public SubmitActionResultDetails(Job job)
        {
            JobId = job.Id;
            ResolutionStatusId = job.ResolutionStatus.Value;
            ResolutionStatusDescription = job.ResolutionStatus.Description;
        }

        public int JobId { get; set; }
        public int ResolutionStatusId { get; set; }
        public string ResolutionStatusDescription { get; set; }
    }
}