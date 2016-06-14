namespace PH.Well.Repositories.Contracts
{
    using System;
    using PH.Well.Domain;
    public interface IFooRepository : IRepository<Foo, int>
    {
    }
}
