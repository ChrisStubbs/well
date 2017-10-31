using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Common.Storage;
using PH.Shared.Well.Data.EF;
using PH.Shared.Well.TranSend.File;
using PH.Shared.Well.TranSend.File.Search;
using PH.Well.Domain.Constants;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    public class EpodGlobalUpliftProcessor
    {
        private readonly GlobalUpliftParser _globalUpliftParser;
        private readonly WellEntities _wellEntities;

        public EpodGlobalUpliftProcessor(GlobalUpliftParser globalUpliftParser, WellEntities wellEntities)
        {
            _globalUpliftParser = globalUpliftParser;
            _wellEntities = wellEntities;
        }

        public void ProcessGlobalUplifts(string sourceFolders, DateTime startDate, DateTime endDate, SearchCriteria searchCriteria)
        {
            List<GlobalUpliftSearchResult> results = new List<GlobalUpliftSearchResult>();
            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                // Allow for {today} & {minus1} - {minusn} macro date replacements
                var source = sourceFolders.Replace("{today}", $"{date:yyyyMMdd}");
                for (int minus = 1; minus < 5; minus++)
                {
                    source = source.Replace($"{{minus{minus}}}", date.AddDays(-minus).ToString("yyyyMMdd"));
                }

                // Check for new (unprocessed) ePod globalUpliftSearchResults
                // Process those globalUpliftSearchResults
                StoreGlobalUpliftResults(SearchEpodFiles(source, date, searchCriteria));
            }
        }

        /// <summary>
        /// Search for globalUpliftSearchResults in archives matching the specified date and parse based on search criteria
        /// </summary>
        /// <param name="date"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        private IEnumerable<GlobalUpliftSearchResult> SearchEpodFiles(string sourceFolders, DateTime date, SearchCriteria searchCriteria)
        {
            var files = Storage.GetFiles(sourceFolders).GroupBy(x=>x.FullName).Select(y=>y.First()).ToList();
            GlobalUpliftParser parser = new GlobalUpliftParser();
            foreach (var file in files)
            {
                if (file.Size > 0)
                {
                    var stream = Storage.ReadFile(file.FullName);
                    foreach (var globalUpliftSearchResult in parser.Parse(file.Name, stream, date, searchCriteria))
                    {
                        yield return globalUpliftSearchResult;
                    }
                }
            }
        }

        /// <summary>
        /// Persist a series of Global Uplift entries to our database
        /// </summary>
        /// <param name="globalUpliftSearchResults"></param>
        /// <remarks>
        /// If no matching Global uplift exists for the CsfReference, look for a matching Global Uplift with no reference (but all other fields match)
        /// If no Global uplift exists, create a new one.
        /// </remarks>
        private void StoreGlobalUpliftResults(IEnumerable<GlobalUpliftSearchResult> globalUpliftSearchResults)
        {
            foreach (var searchResult in globalUpliftSearchResults)
            {
                var attempt = _wellEntities.GlobalUpliftAttempt.FirstOrDefault(
                    x => x.GlobalUplift.CsfReference == searchResult.CsfReference);
                if (attempt == null)
                {
                    // Look for a matching Global uplift with no CsfReference but everything else matches
                    var globalUplift = _wellEntities.GlobalUplift.FirstOrDefault(
                        x =>
                            x.BranchId == searchResult.BranchId &&
                            x.PHAccount == searchResult.AccountNumber && x.CsfReference == null);
                    if (globalUplift == null)
                    {
                        globalUplift = new GlobalUplift()
                        {
                            BranchId = searchResult.BranchId,
                            PHAccount = searchResult.AccountNumber,
                            CsfReference = searchResult.CsfReference,
                            DateCreated = DateTime.Now
                        };
                        // Create a dummy new Global uplift as we are missing one that should have existed
                        _wellEntities.GlobalUplift.Add(globalUplift);
                    }
                    // See if we already have this specific attempt (avoid duplicating entries)
                    attempt = globalUplift.GlobalUpliftAttempt.FirstOrDefault(
                        x => x.DateAttempted == searchResult.Date && x.PlannedStopNumber == searchResult.StopId);
                    if (attempt == null)
                    {
                        attempt = new GlobalUpliftAttempt()
                        {
                            DateAttempted = searchResult.Date.Value,
                            CollectedQty = (short)searchResult.CollectedQty,
                            DriverName = searchResult.DriverName,
                            PlannedStopNumber = searchResult.StopId,
                            RouteNumber = searchResult.RouteId,
                            SourceFilename = searchResult.Filename
                        };
                        switch (searchResult.PerformanceStatusCode)
                        {
                            case RouteStatusCode.NotDeparted:
                                attempt.JobStatusId = (int)PH.Well.Domain.Enums.JobStatus.InComplete;
                                break;
                            case RouteStatusCode.InProgress:
                            default:
                                attempt.JobStatusId = (int)PH.Well.Domain.Enums.JobStatus.InComplete;
                                break;
                            case RouteStatusCode.Completed:
                                attempt.JobStatusId = (int)PH.Well.Domain.Enums.JobStatus.Clean;
                                break;
                        }
                        _wellEntities.GlobalUpliftAttempt.Add(attempt);
                    }
                    // TODO: make this per n records and also after loop
                    _wellEntities.SaveChanges();
                }
            }
        }
    }
}
