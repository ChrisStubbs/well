namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface ILineItemExceptionMapper
    {
        IEnumerable<EditLineItemException> Map(IEnumerable<LineItem> lineItems);
    }
}
