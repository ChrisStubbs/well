namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using Contracts;
    using Domain;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using PH.Well.Domain.Enums;
    using System.Linq;

    public class LineItemExceptionMapper : ILineItemExceptionMapper
    {
        public IEnumerable<EditLineItemException> Map(IEnumerable<LineItem> lineItems)

        {
            var result = new List<EditLineItemException>();

            foreach (var line in lineItems)
            {
                var editLineItemException = new EditLineItemException
                {
                    Id = line.Id,
                    ProductNumber = line.ProductCode,
                    Product = line.ProductDescription,
                    Invoiced = line.OriginalDespatchQuantity,
                    Delivered = line.DeliveredQuantity,
                };
                
                editLineItemException.Exceptions = line.LineItemActions
                    .Select(action => new EditLineItemExceptionDetail
                    {
                        Id = action.Id,
                        LineItemId = action.LineItemId,
                        Originator = action.Originator.Description(),
                        Exception = action.ExceptionType.Description(),
                        Quantity = action.Quantity,
                        Source = action.Source.Description(),
                        Reason = action.Reason.Description(),
                        Erdd = action.ReplanDate,
                        ActionedBy = action.ActionedBy,
                        ApprovedBy = action.ApprovedBy

                    })
                    .ToList();

                result.Add(editLineItemException);
            }

            return result;
        }
    }
}