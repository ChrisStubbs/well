﻿namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.ValueObjects;

    public class LineItemExceptionMapper : ILineItemExceptionMapper
    {
        public IEnumerable<EditLineItemException> Map(IEnumerable<LineItem> lineItems)
            
        {
            var result = new List<EditLineItemException>();

            foreach (var line in lineItems)
            {
                if (line.LineItemActions.Count == 0)
                {
                    var editLineItemException = new EditLineItemException
                    {
                        Id = line.Id,
                        ProductNumber = line.ProductCode,
                        Product = line.ProductDescription,
                        Invoiced = line.OriginalDespatchQuantity,
                        Delivered = line.DeliveredQuantity,
                    };

                    result.Add(editLineItemException);

                }
                else
                {
                    foreach (var action in line.LineItemActions)
                    {
                        var editLineItemException = new EditLineItemException
                        {
                            Id = line.Id,
                            ProductNumber = line.ProductCode,
                            Product = line.ProductDescription,
                            Originator = EnumExtensions.GetDescription(action.Originator), 
                            Exception = EnumExtensions.GetDescription(action.ExceptionType),
                            Invoiced = line.OriginalDespatchQuantity,
                            Delivered = line.DeliveredQuantity,
                            Quantity = action.Quantity,
                            Source = EnumExtensions.GetDescription(action.Source),
                            Reason = EnumExtensions.GetDescription(action.Reason),
                            Erdd = action.ReplanDate,
                            ActionedBy = action.ActionedBy,
                            ApprovedBy = action.ApprovedBy

                        };
                        result.Add(editLineItemException);
                    }
                }
            }

            return result;
        }
    }
}