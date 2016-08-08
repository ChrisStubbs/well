namespace PH.Well.Repositories
{
    using System.Data;

    using Newtonsoft.Json;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class ExceptionEventRepository : IExceptionEventRepository
    {
        private readonly IDapperProxy dapperProxy;

        public ExceptionEventRepository(IDapperProxy dapperProxy)
        {
            this.dapperProxy = dapperProxy;
        }

        public void InsertCreditEvent(CreditEvent creditEvent)
        {
            var creditEventJson = JsonConvert.SerializeObject(creditEvent);

            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", creditEventJson, DbType.String, size: 2500)
                .AddParameter("ExceptionActionId", ExceptionAction.Credit, DbType.Int32)
                .Execute();
        }
    }
}