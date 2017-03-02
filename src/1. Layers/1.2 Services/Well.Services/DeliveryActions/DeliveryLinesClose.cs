namespace PH.Well.Services.DeliveryActions
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class DeliveryLinesClose : BaseDeliveryAction, IDeliveryLinesAction
    {
        public DeliveryAction Action => DeliveryAction.Close;

        public ProcessDeliveryActionResult Execute(Job job)
        {
            var lines = GetJobDetailsByAction(job, Action);

            if (lines.Any())
            {
                UpdateJobDetailStatuses(lines);
            }

            return new ProcessDeliveryActionResult();
        }

        private void UpdateJobDetailStatuses(IEnumerable<JobDetail> jobDetails)
        {
            foreach (var jobDetail in jobDetails)
            {
                if (jobDetail.ShortsAction == DeliveryAction.Close)
                {
                    jobDetail.ShortsStatus = JobDetailStatus.Res;
                }
                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    if (jobDetailDamage.DamageAction == DeliveryAction.Close)
                    {
                        jobDetailDamage.DamageStatus = JobDetailStatus.Res;
                    }
                }
            }
        }
    }
}
