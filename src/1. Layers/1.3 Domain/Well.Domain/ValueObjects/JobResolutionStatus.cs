namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class JobIdResolutionStatus
    {
        public JobIdResolutionStatus(int jobId, ResolutionStatus status)
        {
            JobId = jobId;
            Status = status;
        }
        public int JobId { get; set; }
        public ResolutionStatus Status { get; set; }
    }
}