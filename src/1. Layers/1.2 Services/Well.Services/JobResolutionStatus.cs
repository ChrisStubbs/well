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

            //DriverCompleted
            this.evaluators.Add(job =>
            {
                if (!job.LineItems.SelectMany(p => p.LineItemActions).Any())
                    return ResolutionStatus.DriverCompleted;

                return null;
            });

            //ActionRequired
            this.evaluators.Add(job => 
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any())
                {
                    if (actions.Any(p => p.DeliveryAction == DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.ActionRequired;
                    }
                }

                return null;
            });

            //PendingSubmission
            this.evaluators.Add(job => 
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus <= ResolutionStatus.PendingSubmission)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.PendingSubmission;
                    }
                }

                return null;
            });

            //PendingApproval
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.PendingApproval)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.PendingApproval;
                    }
                }

                return null;
            });

            //Approved
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.Approved)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.Approved;
                    }
                }

                return null;
            });

            //Credited
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.Credited)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined) && actions.Any(p=> p.DeliveryAction == DeliveryAction.Credit))
                    {
                        return ResolutionStatus.Credited;
                    }
                }

                return null;
            });

            //Resolved
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.Resolved)
                {
                    if (actions.All(p => p.DeliveryAction == DeliveryAction.Close))
                    {
                        return ResolutionStatus.Resolved;
                    }
                }

                return null;
            });
        }

        public ResolutionStatus GetStatus(Job job)
        {
            return this.evaluators
                .Select(p => p(job))
                .FirstOrDefault(p => p != null) ?? ResolutionStatus.Invalid;
        }

        public ResolutionStatus StepForward(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
