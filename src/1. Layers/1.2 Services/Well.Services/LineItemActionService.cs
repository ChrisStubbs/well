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
        private readonly IJobResolutionStatus jobResolutionStatus;
        private readonly IJobService jobService;
        private readonly ICommentReasonRepository commentReasonRepository;

        public LineItemActionService(
            ILineItemActionRepository lineItemActionRepository,
            ILineItemSearchReadRepository lineItemRepository,
            ILineItemActionCommentRepository commentRepository,
            IJobRepository jobRepository,
            IJobResolutionStatus jobResolutionStatus,
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

                    // Create default comment for close action
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
                    var deleteDate = DateTime.Now;
                    DeleteComments(itemToDelete, deleteDate);
                    itemToDelete.DateDeleted = deleteDate;
                    lineItemActionRepository.Update(itemToDelete);
                }

                job = GetJob(job.Id);
                job.ResolutionStatus = jobResolutionStatus.GetCurrentResolutionStatus(job);
                jobRepository.Update(job);

                transactionScope.Complete();
            }

            return this.lineItemRepository.GetById(lineItemId);

        }

        private void DeleteComments(LineItemAction itemToDelete, DateTime deleteDate)
        {
            foreach (var comment in itemToDelete.Comments)
            {
                comment.DateDeleted = deleteDate;
                commentRepository.Update(comment);
            }
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

    }
}