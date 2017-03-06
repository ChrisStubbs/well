namespace PH.Well.Services.DeliveryActions
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.Enums;

    public abstract class BaseDeliveryAction
    {
        protected IEnumerable<JobDetail> GetJobDetailsByAction(Job job, DeliveryAction action)
        {
            return job.JobDetails.Where(l => l.ShortsAction == action ||
                                            l.JobDetailDamages.Any(d => d.DamageAction == action));
        }
    }
}
