namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;

    public class LineItemExceptionMapper : ILineItemExceptionMapper
    {
        public IEnumerable<EditLineItemException> Map(IEnumerable<LineItem> lineItems)
            
        {
            var result = new List<EditLineItemException>();

            foreach (var line in lineItems)
            {
                foreach (var action in line.LineItemActions)
                {
                    var editLineItemException = new EditLineItemException
                    {
                        Id = line.Id,
                        ProductNumber = line.ProductCode,
                        Product = line.ProductDescription,
                        Originator = action.Originator ?? "Customer",
                        Exception = action.ExceptionType,
                        Invoiced = line.OriginalDespatchQuantity,
                        Delivered = line.DeliveredQuantity,
                        Quantity = action.Quantity,
                        Source = action.Source,
                        Reason = action.Reason,
                        Erdd = action.ReplanDate,
                        ActionedBy = action.ActionedBy,
                        ApprovedBy = action.ApprovedBy

                    };

                    result.Add(editLineItemException);
                }
            }

            return result;
        }
    }
}