
namespace PH.Well.Repositories
{
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    public class FooRepository : DapperRepository<Foo, int> , IFooRepository
    {
        public FooRepository(ILogger logger, IDapperProxy dapperProxy) : base(logger, dapperProxy)
        {

        }
    }
}
