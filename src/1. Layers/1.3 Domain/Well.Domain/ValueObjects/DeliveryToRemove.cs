namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Domain.Enums;

    public class RouteToRemove
    {
        public RouteToRemove()
        {
            this.RouteHeaders = new List<RouteHeaderToRemove>();   
        }

        public int RouteId { get; set; }

        public List<RouteHeaderToRemove> RouteHeaders { get; set; }

        public DateTime? DateDeleted { get; set; }
        public bool IsDeleted => DateDeleted.HasValue;

        public void SetToDelete()
        {
            // DIJ faster to do !any!deleted e.g. this.IsDeleted = !this.RouteHeaders.Any(x => !x.IsDeleted);
            if (this.RouteHeaders.All(x => x.IsDeleted))
            {
                this.DateDeleted = DateTime.Now;
            }
        }
    }

    public class RouteHeaderToRemove
    {
        public RouteHeaderToRemove()
        {
            this.Stops = new List<StopToRemove>();
        }

        public int RouteHeaderId { get; set; }

        public int RouteId { get; set; }

        public int BranchId { get; set; }

        public List<StopToRemove> Stops { get; set; }

        public DateTime? DateDeleted { get; set; }
        public bool IsDeleted => DateDeleted.HasValue;

        public void SetToDelete()
        {
            // DIJ faster to do !any!deleted e.g. this.IsDeleted = !this.Stops.Any(x => !x.IsDeleted);
            if (this.Stops.All(x => x.IsDeleted))
            {
                this.DateDeleted = DateTime.Now;
            }
        }
    }

    public class StopToRemove
    {
        public StopToRemove()
        {
            this.Jobs = new List<JobToRemove>();
        }

        public int StopId { get; set; }

        public int RouteHeaderId { get; set; }

        public List<JobToRemove> Jobs { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool IsDeleted => DateDeleted.HasValue;

        public void SetToDelete()
        {
            // DIJ faster to do !any!deleted e.g. this.IsDeleted = !this.Jobs.Any(x => !x.IsDeleted);
            if (this.Jobs.All(x => x.IsDeleted))
            {
                this.DateDeleted = DateTime.Now;
            }
        }
    }

    public class JobToRemove
    {
        public JobToRemove()
        {
            this.JobDetails = new List<JobDetailToRemove>();
        }

        public int JobId { get; set; }

        public int StopId { get; set; }

        public string RoyaltyCode { get; set; }

        public List<JobDetailToRemove> JobDetails { get; set; }

        public DateTime? DateDeleted { get; set; }
        public bool IsDeleted => DateDeleted.HasValue;

        public void SetToDelete()
        {
            // DIJ faster to do !any!deleted e.g. this.IsDeleted = !this.JobDetails.Any(x => !x.IsDeleted);
            if (this.JobDetails.All(x => x.IsDeleted))
            {
                this.DateDeleted = DateTime.Now;
            }
        }
    }

    public class JobDetailToRemove
    {
        public JobDetailToRemove()
        {
            this.JobDamages = new List<JobDamageToRemove>();
        }

        public int JobId { get; set; }

        public int JobDetailId { get; set; }

        public JobDetailStatus ShortsStatus { get; set; }
        
        public List<JobDamageToRemove> JobDamages { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool IsDeleted => DateDeleted.HasValue;

        public DateTime DateUpdated { get; set; }

        public void SetToDelete()
        {
            if (this.ShortsStatus == JobDetailStatus.Res)
            {
                this.DateDeleted = DateTime.Now;

                foreach (var damage in this.JobDamages)
                {
                    if (damage.DamageStatus == JobDetailStatus.Res)
                    {
                        damage.DateDeleted = DateTime.Now;
                    }
                }
            }
        }
    }

    public class JobDamageToRemove
    {
        public int JobDamageId { get; set; }

        public int JobDetailId { get; set; }

        public DateTime? DateDeleted { get; set; }

        public bool IsDeleted => DateDeleted.HasValue;

        public JobDetailStatus DamageStatus { get; set; }
    }
}