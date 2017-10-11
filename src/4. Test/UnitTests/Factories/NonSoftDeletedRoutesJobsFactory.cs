using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.UnitTests.Factories
{
    public class NonSoftDeletedRoutesJobsFactory : EntityFactory<NonSoftDeletedRoutesJobsFactory, JobForClean>
    {
        public NonSoftDeletedRoutesJobsFactory()
        {
            this.Entity.BranchId = 55;
            this.Entity.JobId = 300;
            this.Entity.JobRoyaltyCode = null;
            this.Entity.JobRoyaltyCodeId = null;
            this.Entity.ResolutionStatusId = ResolutionStatus.Closed | ResolutionStatus.DriverCompleted;
            this.Entity.RouteDate = DateTime.Now;
            this.Entity.RouteId = 20;
            this.Entity.StopId = 33;
        }
    }
}
