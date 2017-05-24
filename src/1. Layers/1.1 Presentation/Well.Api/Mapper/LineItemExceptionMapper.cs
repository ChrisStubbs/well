namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using Contracts;
    using Domain;
    using Domain.Extensions;
    using Domain.ValueObjects;

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

                foreach (var action in line.LineItemActions)
                {
                    var editLineItemExceptionDetail = new EditLineItemExceptionDetail
                    {
                        Originator = EnumExtensions.GetDescription(action.Originator),
                        Exception = EnumExtensions.GetDescription(action.ExceptionType),
                        Quantity = action.Quantity,
                        Source = EnumExtensions.GetDescription(action.Source),
                        Reason = EnumExtensions.GetDescription(action.Reason),
                        Erdd = action.ReplanDate,
                        ActionedBy = action.ActionedBy,
                        ApprovedBy = action.ApprovedBy

                    };

                    editLineItemException.Exceptions.Add(editLineItemExceptionDetail);

                }

                result.Add(editLineItemException);
            }

            return result;
        }
    }
}