using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;
using PH.Well.Domain.Enums;

namespace PH.Well.Services
{
    public class JobResolutionStatus : IJobResolutionStatus
    {
        private readonly List<Func<Job, ResolutionStatus>> evaluators;

        public JobResolutionStatus()
        {
            this.evaluators = new List<Func<Job, ResolutionStatus>>();
            this.evaluators.Add(job =>
            {
                if (job.LineItems.SelectMany(p=> p.LineItemActions).All(p=> p.DeliveryAction == DeliveryAction.NotDefined))
                    return ResolutionStatus.DriverCompleted;

                return null;
            });
        }

        public ResolutionStatus GetStatus(Job job)
        {
            var result = this.evaluators
                .Select(p => p(job))
                .FirstOrDefault(p => p != null);
            
            return result ?? ResolutionStatus.Invalid;
        }
    }
}
