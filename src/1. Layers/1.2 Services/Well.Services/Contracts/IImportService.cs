using System.Collections.Generic;
using PH.Well.Domain;

namespace PH.Well.Services
{
    using Contracts;

    public interface IImportService
    {
        void ImportStops(RouteHeader fileRouteHeader, IImportMapper importMapper, IImportCommands importCommands);

        void DeleteJobs(IList<Job> jobsToBeDeleted);
    }
}