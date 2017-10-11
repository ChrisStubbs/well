namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Domain.ValueObjects;

    public class WellCleanUpRepository : IWellCleanUpRepository
    {
        private readonly IDapperProxy dapperProxy;
        private readonly IUserNameProvider userNameProvider;

        public WellCleanUpRepository(IDapperProxy dapperProxy, IUserNameProvider userNameProvider)
        {
            this.dapperProxy = dapperProxy;
            this.userNameProvider = userNameProvider;
        }

        public IList<JobForClean> GetJobsAvailableForClean()
        {
            var strSQL = "SELECT RouteDate, JobId, StopId, RouteId, ResolutionStatus, BranchId, RoyaltyCode FROM JobsAvailableForCleanView";
            var result = new List<JobForClean>();

            Action<IDataReader> callBack = reader =>
            {
                var royaltyCodeIndex = reader.GetOrdinal("RoyaltyCode");

                while (reader.Read())
                {
                    result.Add(new JobForClean
                    {
                        BranchId = reader.GetInt32(reader.GetOrdinal("BranchId")),
                        JobId = reader.GetInt32(reader.GetOrdinal("JobId")),
                        ResolutionStatusId = reader.GetInt32(reader.GetOrdinal("ResolutionStatus")),
                        RouteId = reader.GetInt32(reader.GetOrdinal("RouteId")),
                        StopId = reader.GetInt32(reader.GetOrdinal("StopId")),
                        RouteDate = reader.GetDateTime(reader.GetOrdinal("RouteDate")),
                        JobRoyaltyCode = reader.IsDBNull(royaltyCodeIndex) ? null : reader.GetString(royaltyCodeIndex)
                    });
                }
            };

            this.dapperProxy.ExecuteSql(strSQL, null, callBack);

            return result;
        }

        public void CleanStops()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.ArchiveStops)
                .AddParameter("ArchiveDate", DateTime.Now, DbType.DateTime)
                .Execute();
        }

   
        public void CleanRouteHeader()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.ArchiveRouteHeader)
                .AddParameter("ArchiveDate", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public void CleanRoutes()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.ArchiveRoutes)
                .AddParameter("ArchiveDate", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public void CleanActivities()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.ArchiveActivity)
                .AddParameter("ArchiveDate", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public void CleanJobs(IList<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.ArchiveJobs)
                .AddParameter("JobIds", jobIds.ToIntDataTables("JobIds"), DbType.Object)
                .AddParameter("ArchiveDate", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public void UpdateStatistics()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.UpdateStatistics).Execute();
        }

        public void CleanExceptionEvents()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.ArchiveExceptionEvent)
                .AddParameter("ArchiveDate", DateTime.Now, DbType.DateTime)
                .Execute();
        }
    }
}
