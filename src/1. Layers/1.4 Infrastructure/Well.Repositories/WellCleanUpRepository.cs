using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Common.Extensions;

namespace PH.Well.Repositories
{
    public class WellCleanUpRepository : IWellCleanUpRepository
    {
        private readonly IDapperProxy dapperProxy;

        public WellCleanUpRepository(IDapperProxy dapperProxy)
        {
            this.dapperProxy = dapperProxy;
        }

        public IList<NonSoftDeletedRoutesJobs> GetNonSoftDeletedRoutes()
        {
            var strSQL = "SELECT RouteDate, JobId, StopId, RouteId, ResolutionStatus, BranchId, RoyaltyCode FROM NonSoftDeletedRoutesJobsView";
            var result = new List<NonSoftDeletedRoutesJobs>();

            Action<IDataReader> callBack = reader =>
            {
                var royaltyCodeIndex = reader.GetOrdinal("RoyaltyCode");

                while (reader.Read())
                {
                    result.Add(new NonSoftDeletedRoutesJobs
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

        private void DeleteJobs(IList<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.CleanJobs)
                .AddParameter("JobIds", jobIds.ToIntDataTables("JobIds"), DbType.Object)
                .AddParameter("DateDeleted", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        private void DeleteStops(IList<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.CleanStops)
                .AddParameter("JobIds", jobIds.ToIntDataTables("JobIds"), DbType.Object)
                .AddParameter("DateDeleted", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        private void DeleteRoutes(IList<int> jobIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.CleanRoutes)
                .AddParameter("JobIds", jobIds.ToIntDataTables("JobIds"), DbType.Object)
                .AddParameter("DateDeleted", DateTime.Now, DbType.DateTime)
                .Execute();
        }

        public Task SoftDelete(IList<int> jobIds)
        {
            return Task.Run(() =>
            {
                this.DeleteJobs(jobIds);
                this.DeleteStops(jobIds);
                this.DeleteRoutes(jobIds);
            });
        }
    }
}
