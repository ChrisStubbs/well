using System;

namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class LineItemActionService : ILineItemActionService
    {
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly ILineItemSearchReadRepository lineItemRepository;
        private readonly ILineItemActionCommentRepository commentRepository;
        private readonly IJobRepository jobRepository;
        private readonly IGetJobResolutionStatus jobResolutionStatus;
        private readonly IJobService jobService;
        private readonly ICommentReasonRepository commentReasonRepository;

        private static readonly JobType[] ActionableJobTypes =
        {
            JobType.Tobacco,
            JobType.Ambient,
            JobType.Alcohol,
            JobType.Chilled,
            JobType.Frozen,
            JobType.GlobalUplift,
            JobType.StandardUplift
        };

        public LineItemActionService(
            ILineItemActionRepository lineItemActionRepository,
            ILineItemSearchReadRepository lineItemRepository,
            ILineItemActionCommentRepository commentRepository,
            IJobRepository jobRepository,
            IGetJobResolutionStatus jobResolutionStatus,
            IJobService jobService,
            ICommentReasonRepository commentReasonRepository)
        {
            this.lineItemActionRepository = lineItemActionRepository;
            this.lineItemRepository = lineItemRepository;
            this.commentRepository = commentRepository;
            this.jobRepository = jobRepository;
            this.jobResolutionStatus = jobResolutionStatus;
            this.jobService = jobService;
            this.commentReasonRepository = commentReasonRepository;
        }

        public LineItem SaveLineItemActions(Job job, int lineItemId, IEnumerable<LineItemAction> lineItemActions)
        {
            var lineItem = this.lineItemRepository.GetById(lineItemId);

            if (lineItem == null)
            {
                return null;
            }
            var itemActions = lineItemActions as LineItemAction[] ?? lineItemActions.ToArray();

            using (var transactionScope = new TransactionScope())
            {
                foreach (var action in itemActions)
                {
                    var original = lineItem.LineItemActions.FirstOrDefault(x => x.Id == action.Id);

                    // Create default comment for close action every time when action is saved
                    if (action.DeliveryAction == DeliveryAction.Close)
                    {
                        var defaultCommentReason = commentReasonRepository.GetAll().Single(x => x.IsDefault);
                        action.Comments.Add(new LineItemActionComment
                        {
                            CommentDescription = defaultCommentReason.Description,
                            CommentReasonId = defaultCommentReason.Id,
                            ToQty = 0
                        });
                    }

                    if (action.IsTransient())
                    {
                        action.LineItemId = lineItemId;
                        lineItemActionRepository.Save(action);
                    }
                    else
                    {
                        if (original != null && original.HasChanges(action))
                        {
                            lineItemActionRepository.Update(action);
                        }
                    }

                    foreach (var comment in action.Comments.Where(x => x.IsTransient()))
                    {
                        comment.LineItemActionId = action.Id;
                        comment.FromQty = original?.Quantity;
                        comment.ToQty = action.Quantity;
                        commentRepository.Save(comment);
                    }
                }

                foreach (var itemToDelete in lineItem.LineItemActions.Where(x => !itemActions.Select(y => y.Id).Contains(x.Id)
                                                                                && x.Originator != Originator.Driver))
                {
                    lineItemActionRepository.Delete(itemToDelete);
                }

                job = GetJob(job.Id);
                job.ResolutionStatus = jobResolutionStatus.GetCurrentResolutionStatus(job);
                jobRepository.SaveJobResolutionStatus(job);
                jobRepository.Update(job);
                // Compute well status
                jobService.ComputeAndPropagateWellStatus(job);

                transactionScope.Complete();
            }

            return this.lineItemRepository.GetById(lineItemId);

        }

        private Job GetJob(int jobId )
        {
            var job = jobRepository.GetById(jobId);
            return jobService.PopulateLineItemsAndRoute(job);
        }

        public LineItem InsertLineItemActions(LineItemActionUpdate lineItemActionUpdate)
        {

            var lineItemAction = new LineItemAction
            {
                LineItemId = lineItemActionUpdate.LineItemId,
                DeliveryAction = lineItemActionUpdate.DeliverAction,
                ExceptionType = lineItemActionUpdate.ExceptionType,
                Quantity = lineItemActionUpdate.Quantity,
                Source = lineItemActionUpdate.Source,
                Reason = lineItemActionUpdate.Reason,
                Originator = lineItemActionUpdate.Orginator
            };

            lineItemActionRepository.Save(lineItemAction);

            return lineItemRepository.GetById(lineItemAction.LineItemId);
        }

        public LineItem UpdateLineItemActions(LineItemActionUpdate lineItemActionUpdate)
        {
            var lineItemAction = new LineItemAction
            {
                Id = lineItemActionUpdate.Id,
                LineItemId = lineItemActionUpdate.LineItemId,
                DeliveryAction = lineItemActionUpdate.DeliverAction,
                ExceptionType = lineItemActionUpdate.ExceptionType,
                Quantity = lineItemActionUpdate.Quantity,
                Source = lineItemActionUpdate.Source,
                Reason = lineItemActionUpdate.Reason,
                Originator = lineItemActionUpdate.Orginator
            };

            lineItemActionRepository.Update(lineItemAction);
            return lineItemRepository.GetById(lineItemAction.LineItemId);
        }

        /// <summary>
        /// Checks if job is of type that can be actioned otherwise throw exception
        /// </summary>
        /// <param name="job"></param>
        /// <exception cref="ArgumentOutOfRangeException">When job is not actionable</exception>
        private void CheckJobIsActionable(Job job)
        {
            if (!ActionableJobTypes.Contains(job.JobType))
            {
                throw new ArgumentOutOfRangeException(nameof(job.JobType), job.JobType,
                    $"{job.JobType} is not valid to action");
            }
        }

        public bool CanSetActionForJob(Job job, DeliveryAction deliveryAction)
        {
            // Validate job type for which actions can be set
            CheckJobIsActionable(job);

            // Global uplift jobs can not be credited or Pod hence should only be closed
            if (job.JobType == JobType.GlobalUplift)
            {
                if (deliveryAction == DeliveryAction.Credit || deliveryAction == DeliveryAction.Pod)
                {
                    return false;
                }
                return true;
            }

            if (job.IsProofOfDelivery)
            {
                if (deliveryAction == DeliveryAction.Credit)
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (deliveryAction == DeliveryAction.Pod)
                {
                    return false;
                }
                return true;
            }
        }

    }
}