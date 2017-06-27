//namespace PH.Well.Services
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Transactions;
//    using Common.Contracts;
//    using Contracts;
//    using Domain;
//    using Domain.Enums;
//    using Domain.ValueObjects;
//    using Repositories.Contracts;

//    public class BulkActionService : IBulkActionService
//    {
//        private readonly ILogger logger;
//        private readonly ILineItemSearchReadRepository lineItemRepository;
//        private readonly ILineItemActionRepository lineItemActionRepository;
//        private readonly ILineItemActionCommentRepository commentRepository;
//        private readonly IJobRepository jobRepository;

//        public BulkActionService(
//            ILogger logger,
//            ILineItemSearchReadRepository lineItemRepository,
//            ILineItemActionRepository lineItemActionRepository,
//            ILineItemActionCommentRepository commentRepository,
//            IJobRepository jobRepository)
//        {
//            this.logger = logger;
//            this.lineItemRepository = lineItemRepository;
//            this.lineItemActionRepository = lineItemActionRepository;
//            this.commentRepository = commentRepository;
//            this.jobRepository = jobRepository;
//        }
//        public BulkActionResult Add(BulkAddModel bulkAddModel)
//        {
//            List<LineItem> lineItems = GetLineItems(bulkAddModel);
//            List<int> lineItemIdsToIgnore = new List<int>();
//            List<LineItem> lineItemsToProcess = new List<LineItem>();

//            try
//            {
//                using (var transactionScope = new TransactionScope())
//                {
//                    switch (bulkAddModel.Option)
//                    {
//                        case BulkAddOptions.Overwrite:
//                            CloseAllExistingActions(lineItems);
//                            break;
//                        case BulkAddOptions.Ignore:
//                            lineItemIdsToIgnore = GetLineItemsWithActions(lineItems);
//                            break;
//                        default:
//                            throw new ArgumentOutOfRangeException();
//                    }

//                    lineItemsToProcess = lineItems.Where(x => !lineItemIdsToIgnore.Contains(x.Id)).ToList();

//                    foreach (var lineItem in lineItemsToProcess)
//                    {
//                        var lineItemAction = new LineItemAction
//                        {
//                            LineItemId = lineItem.Id,
//                            DeliveryAction = bulkAddModel.DeliverAction,
//                            ExceptionType = bulkAddModel.ExceptionType,
//                            Quantity = lineItem.OriginalDespatchQuantity ?? 0,
//                            Source = bulkAddModel.Source,
//                            Reason = bulkAddModel.Reason,
//                            Originator = Originator.Customer
//                        };

//                        lineItemActionRepository.Save(lineItemAction);
//                    }

//                    transactionScope.Complete();
//                }

//                return new BulkActionResult
//                {
//                    IsValid = true,
//                    Message = $"Successfully added  {lineItemsToProcess.Count } exceptions, Ignored {lineItemIdsToIgnore.Count}."
//                };
//            }
//            catch (Exception ex)
//            {
//                logger.LogError($"Error Bulk Adding actions for bulk add Model", ex);
//                return new BulkActionResult { Message = "Error with Bulk Adding. No exceptions have been added" };
//            }

//        }

//        private List<int> GetLineItemsWithActions(List<LineItem> lineItems)
//        {
//            return lineItems.SelectMany(x => x.LineItemActions)
//                 .Where(x => x.Quantity > 0)
//                 .Select(x => x.LineItemId).Distinct().ToList();
//        }

//        public BulkActionSummary GetAddSummary(BulkAddModel bulkAddModel)
//        {
//            List<LineItem> lineItems = GetLineItems(bulkAddModel);
//            List<int> lineItemIdsWithActions = GetLineItemsWithActions(lineItems);

//            var lineItemsWithWarnings = lineItems.Where(x => lineItemIdsWithActions.Contains(x.Id)).ToList();
//            var summary = new BulkActionSummary();

//            if (lineItemsWithWarnings.Any())
//            {
//                summary.Message = $"The following {lineItemsWithWarnings.Count} lines already have line actions defined";
//                var jobs = jobRepository.GetByIds(lineItemsWithWarnings.Select(x => x.JobId).Distinct()).ToArray();
//                foreach (var lineItem in lineItemsWithWarnings)
//                {
//                    var job = jobs.Single(x => x.Id == lineItem.JobId);

//                    summary.WarningItems.Add(
//                        new BulkActionSummaryItem
//                        {
//                            Identifier = $"Invoice {job.InvoiceNumber} , Product: {lineItem.ProductCode} - {lineItem.ProductDescription} ,",
//                            ShortQuantity = lineItem.TotalShortQty,
//                            DamageQuantity = lineItem.TotalDamageQty,
//                            BypassQuantity = lineItem.TotalBypassQty,
//                        });
//                }

//            }

//            return summary;
//        }

//        public virtual void CloseAllExistingActions(List<LineItem> lineItems)
//        {
//            foreach (var action in lineItems.SelectMany(x => x.LineItemActions))
//            {
//                var comment = new LineItemActionComment
//                {
//                    LineItemActionId = action.Id,
//                    CommentReasonId = LineItemActionComment.CommentReasonIdBulkUpdate,
//                    FromQty = action.Quantity,
//                    ToQty = 0,
//                };
//                commentRepository.Save(comment);

//                action.DeliveryAction = DeliveryAction.Close;
//                action.Quantity = 0;
//                lineItemActionRepository.Update(action);
//            }
//        }

//        public virtual List<LineItem> GetLineItems(BulkAddModel bulkAddModel)
//        {
//            if (bulkAddModel.JobIds.Any())
//            {
//                return lineItemRepository.GetLineItemByJobIds(bulkAddModel.JobIds).ToList();
//            }
//            return lineItemRepository.GetLineItemByIds(bulkAddModel.LineItemIds).ToList();
//        }
//    }
//}