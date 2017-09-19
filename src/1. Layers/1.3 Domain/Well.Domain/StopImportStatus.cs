namespace PH.Well.Domain
{
    public class StopImportStatus
    {
        public enum Status
        {
            New = 0,
            Updated = 1,
            IgnoredAsCompleted = 2
        }

        public StopImportStatus(Stop stop, Status importStatus)
        {
            Stop = stop;
            ImportStatus = importStatus;
        }
        public Stop Stop { get; set; }
        public Status ImportStatus { get; set; }
    }


}