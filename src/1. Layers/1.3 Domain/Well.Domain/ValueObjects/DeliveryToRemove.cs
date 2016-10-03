namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.ObjectModel;

    using PH.Well.Domain.Enums;

    public class RouteToRemove
    {
        public RouteToRemove()
        {
            this.RouteHeaders = new Collection<RouteHeaderToRemove>();   
        }

        public int RouteId { get; set; }

        public Collection<RouteHeaderToRemove> RouteHeaders { get; set; }
    }

    public class RouteHeaderToRemove
    {
        public RouteHeaderToRemove()
        {
            this.Stops = new Collection<StopToRemove>();
        }

        public int RouteHeaderId { get; set; }

        public int RouteId { get; set; }

        public int BranchId { get; set; }

        public Collection<StopToRemove> Stops { get; set; }
    }

    public class StopToRemove
    {
        public StopToRemove()
        {
            this.Jobs = new Collection<JobToRemove>();
        }

        public int StopId { get; set; }

        public int RouteHeaderId { get; set; }

        public Collection<JobToRemove> Jobs { get; set; }
    }

    public class JobToRemove
    {
        public JobToRemove()
        {
            this.JobDetails = new Collection<JobDetailToRemove>();
        }

        public int JobId { get; set; }

        public int StopId { get; set; }

        public Collection<JobDetailToRemove> JobDetails { get; set; }
    }

    public class JobDamageToRemove
    {
        public int JobDamageId { get; set; }

        public int JobDetailId { get; set; }
    }

    public class JobDetailToRemove
    {
        public JobDetailToRemove()
        {
            this.JobDamages = new Collection<JobDamageToRemove>();
        }

        public int JobDetailId { get; set; }

        public JobDetailStatus JobDetailStatus => (JobDetailStatus)this.JobDetailStatusId;

        public int JobDetailStatusId { get; set; }

        public Collection<JobDamageToRemove> JobDamages { get; set; }
    }
}