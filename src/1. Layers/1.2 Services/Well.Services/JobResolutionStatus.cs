using System;
using System.Collections.Generic;
using System.Linq;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class JobResolutionStatus : IJobResolutionStatus
    {
        private readonly List<Func<Job, ResolutionStatus>> evaluators;
        private readonly IUserThresholdService userThresholdService;
        private readonly IDateThresholdService dateThresholdService;
        private readonly Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> steps;

        public JobResolutionStatus(IUserThresholdService userThresholdService, IDateThresholdService dateThresholdService)
        {
            this.evaluators = new List<Func<Job, ResolutionStatus>>();
            this.steps = new Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>>();
            this.userThresholdService = userThresholdService;
            this.dateThresholdService = dateThresholdService;

            this.fillEvaluators();
            this.fillSteps();
        }

        private void fillSteps()
        {
            steps.Add(ResolutionStatus.Imported, job => ResolutionStatus.DriverCompleted);

            steps.Add(ResolutionStatus.DriverCompleted, job =>
            {
                if (job.LineItems.SelectMany(p => p.LineItemActions).Any())
                {
                    return ResolutionStatus.ActionRequired;
                }
                else if (dateThresholdService.EarliestSubmitDate(job.JobRoute.RouteDate, job.JobRoute.BranchId) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | ResolutionStatus.DriverCompleted;
                }

                return ResolutionStatus.DriverCompleted;
            });

            steps.Add(ResolutionStatus.ActionRequired, job =>
            {
                return GetStatus(job);
            });
            
            steps.Add(ResolutionStatus.PendingSubmission, job =>
            {

                if (this.userThresholdService.UserHasRequiredCreditThreshold(job))
                {
                    return ResolutionStatus.Approved;
                }

                return ResolutionStatus.PendingApproval;
            });

            steps.Add(ResolutionStatus.Approved, job =>
            {
                if (job.LineItems.SelectMany(p => p.LineItemActions).Any(p => p.DeliveryAction == DeliveryAction.Credit))
                {
                    return ResolutionStatus.Credited;
                }

                return ResolutionStatus.Resolved;
            });

            steps.Add(ResolutionStatus.Credited, job =>
            {
                if (dateThresholdService.EarliestSubmitDate(job.JobRoute.RouteDate, job.JobRoute.BranchId) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | ResolutionStatus.Credited;
                }

                return ResolutionStatus.Credited;
            });

            steps.Add(ResolutionStatus.Resolved, job =>
            {
                if (dateThresholdService.EarliestSubmitDate(job.JobRoute.RouteDate, job.JobRoute.BranchId) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | ResolutionStatus.Resolved;
                }

                return ResolutionStatus.Resolved;
            });
        }

        private void fillEvaluators()
        {
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
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined) && actions.Any(p => p.DeliveryAction == DeliveryAction.Credit))
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
            if (this.steps.ContainsKey(job.ResolutionStatus))
            {
                return this.steps[job.ResolutionStatus](job);
            }

            return ResolutionStatus.Invalid;
        }
    }
}