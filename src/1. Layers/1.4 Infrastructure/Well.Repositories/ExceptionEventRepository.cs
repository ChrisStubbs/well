namespace PH.Well.Repositories
{
    using System;
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

        public void InsertCreditEvent(CreditEvent creditEvent, string username)
        {
            var creditEventJson = JsonConvert.SerializeObject(creditEvent);

            this.dapperProxy.WithStoredProcedure(StoredProcedures.EventInsert)
                .AddParameter("Event", creditEventJson, DbType.String, size: 2500)
                .AddParameter("ExceptionActionId", ExceptionAction.Credit, DbType.Int32)
                .AddParameter("CreatedBy", username, DbType.String, size: 50)
                .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                .AddParameter("UpdatedBy", username, DbType.String, size: 50)
                .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                .Execute();
        }
    }
}