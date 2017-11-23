namespace PH.Well.Domain.ValueObjects
{
    public class UserJobs
    {
        public string UserName { get; set; }

        public int[] JobIds { get; set; }

        public bool AllocatePendingApprovalJobs { get; set; }
    }
}