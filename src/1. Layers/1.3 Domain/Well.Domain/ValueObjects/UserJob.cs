namespace PH.Well.Domain.ValueObjects
{
    public class UserJob
    {
        public UserJob()
        {
        }

        public UserJob(int userId, int jobId)
        {
            UserId = userId;
            JobId = jobId;
        }
        
        public int UserId { get; set; }
        public int JobId { get; set; }
    }
}
