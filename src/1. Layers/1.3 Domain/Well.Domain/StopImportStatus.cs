namespace PH.Well.Domain
{
    using Contracts;

    public class StopImportStatus
    {
        public enum Status
        {
            New = 0,
            Updated = 1,
            IgnoredAsCompleted = 2
        }

        public StopImportStatus(Stop stop, Status importStatus, IStopMoveIdentifiers originalStopIdentifiers = null)
        {
            Stop = stop;
            ImportStatus = importStatus;
            OriginalStopIdentifiers = originalStopIdentifiers ?? new Stop();
        }

        public Stop Stop { get; set; }
        public Status ImportStatus { get; set; }
        public IStopMoveIdentifiers OriginalStopIdentifiers { get; set; }
    }








}