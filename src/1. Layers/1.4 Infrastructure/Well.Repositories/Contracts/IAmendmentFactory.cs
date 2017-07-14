namespace PH.Well.Repositories.Contracts
{
    using Domain.ValueObjects;

    public interface IAmendmentFactory
    {
        AmendmentTransaction Build(Amendment amendment);
    }
}
