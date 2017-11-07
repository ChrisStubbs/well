﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PH.Common.Storage;
using PH.Shared.EmailService.Client.Rest;
using PH.Shared.EmailService.Models;
using PH.Shared.Well.Data.EF;
using PH.Shared.Well.TranSend.File;
using PH.Shared.Well.TranSend.File.Search;
using PH.Well.Domain.Constants;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using Branch = PH.Well.Domain.Enums.Branch;

namespace PH.Well.Task.GlobalUplifts.EpodFiles
{
    public class EpodGlobalUpliftProcessor : ITaskProcessor
    {
        #region Constants
        private const string NOT_AVAILABLE = "*Not available*";
        private const string ALL_BRANCHES_EMAIL = "all_branches@palmerharvey.co.uk";
        #endregion Constants

        #region Private fields
        private List<GlobalUplift> _globalUplifts;
        private readonly GlobalUpliftParser _globalUpliftParser;
        private readonly WellEntities _wellEntities;
        private readonly List<GlobalUpliftAttempt> _globalUpliftAttempts;
        #endregion Private fields

        #region Public properties
        public string Sources { get; set; }
        public List<int> Branches { get; set; }
        #endregion Public properties

        #region Constructors
        public EpodGlobalUpliftProcessor(GlobalUpliftParser globalUpliftParser, WellEntities wellEntities)
        {
            _globalUpliftParser = globalUpliftParser;
            _wellEntities = wellEntities;
            _globalUplifts = _wellEntities.GlobalUplift.Include("GlobalUpliftAttempt").OrderBy(x => x.StartDate).ToList();
            _globalUpliftAttempts = _wellEntities.GlobalUpliftAttempt.ToList();
        }

        #endregion Constructors

        #region Public methods
        public void Run()
        {
            SearchCriteria criteria = new SearchCriteria()
            {
                Branches = this.Branches,
                JobType = "UPL-GLO"
            };
            DateTime dateFrom = DateTime.Today.AddDays(-1);
            dateFrom = new DateTime(2017, 9, 4);
            DateTime dateTo = DateTime.Today;
            dateTo = new DateTime(2017, 10, 9);
            this.ProcessGlobalUplifts(this.Sources, dateFrom, dateTo, criteria);
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
                StoreGlobalUpliftResults(date, SearchEpodFiles(source, date, searchCriteria));
            }
        }
        #endregion Public methods

        #region private helper methods
        /// <summary>
        /// Search for globalUpliftSearchResults in archives matching the specified date and parse based on search criteria
        /// </summary>
        /// <param name="date"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        private IEnumerable<GlobalUpliftSearchResult> SearchEpodFiles(string sourceFolders, DateTime date, SearchCriteria searchCriteria)
        {
            var files = Storage.GetFiles(sourceFolders).GroupBy(x => x.FullName).Select(y => y.First()).Where(x => x.Name.ToLower().StartsWith("epod_")).ToList();
            GlobalUpliftParser parser = new GlobalUpliftParser();
            foreach (var file in files /*.Where(x => x.Name.ToLower() == "ePOD__20170911_11131106715484.xml".ToLower())*/)
            {
                if (file.Size > 0)
                {
                    //Console.WriteLine($"Scanning for Global Uplifts in {file.Name}");
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
        /// <param name="dateProcessed"></param>
        /// <param name="globalUpliftSearchResults"></param>
        /// <remarks>
        /// If no matching Global uplift exists for the CsfReference, look for a matching Global Uplift with no reference (but all other fields match)
        /// If no Global uplift exists, create a new one.
        /// </remarks>
        private void StoreGlobalUpliftResults(DateTime dateProcessed, IEnumerable<GlobalUpliftSearchResult> globalUpliftSearchResults)
        {
            Console.WriteLine($"Processing Global uplifts for {dateProcessed:dd/MM/yyyy}");

            DateTime startWindow = dateProcessed.AddDays(-14);
            DateTime endWindow = startWindow.AddMonths(2);

            int counter = 0;
            foreach (var searchResult in globalUpliftSearchResults /*.Where(x=>x.AccountNumber == "37435.018")*/)
            {
                // Match attempt based on the main properties expected to be unique
                var attempt = _globalUpliftAttempts.FirstOrDefault(
                    x => x.DateAttempted == searchResult.Date &&
                    x.DriverName == searchResult.DriverName &&
                    x.RouteNumber == searchResult.RouteId &&
                    x.PlannedStopNumber == searchResult.StopId);

                if (attempt == null)
                {
                    // Look for a matching Global uplift with same CsfRefernce if present, else no CSF yet with a valid date range
                    var globalUplifts = _globalUplifts.Where(
                        x =>
                            x.BranchId == searchResult.BranchId &&
                            x.PHAccount == searchResult.AccountNumber).ToList();
                    var globalUplift = _globalUplifts.FirstOrDefault(
                        x =>
                            x.BranchId == searchResult.BranchId &&
                            x.PHAccount == searchResult.AccountNumber &&
                            (x.CsfReference == null && (x.StartDate == null || searchResult.Date >= x.StartDate) && (x.EndDate == null || x.EndDate < endWindow)) ||
                            x.CsfReference == searchResult.CsfReference);
                    if (globalUplift == null)
                    {
                        // This is a manually created Global Uplift. Create a dummy 14 day date window.
                        globalUplift = new GlobalUplift()
                        {
                            CsfReference = searchResult.CsfReference,
                            BranchId = searchResult.BranchId,
                            PHAccount = searchResult.AccountNumber,
                            DateCreated = DateTime.Now,
                            SourceFilename = searchResult.Filename,
                            PHProductCode = searchResult.ProductCode,
                            StartDate = searchResult.Date,
                            EndDate = searchResult.Date.Value.AddDays(14)
                        };
                        // Create a dummy new Global uplift as we are missing one that should have existed
                        // Need to force a re-sort in case files were processed out of sequence
                        _wellEntities.GlobalUplift.Add(globalUplift);
                        _globalUplifts.Add(globalUplift);
                        _globalUplifts = _globalUplifts.OrderBy(x => x.StartDate).ToList();
                        Console.WriteLine($"Adding missing Global uplift {searchResult.BranchId:##} {searchResult.AccountNumber}");
                        counter++;
                    }

                    if (globalUplift.CsfReference == null)
                    {
                        globalUplift.CsfReference = searchResult.CsfReference;
                    }

                    // See if we already have this specific attempt (avoid duplicating entries)
                    attempt = globalUplift.GlobalUpliftAttempt.FirstOrDefault(
                        x => x.DateAttempted == searchResult.Date && x.PlannedStopNumber == searchResult.StopId);
                    if (attempt == null)
                    {
                        attempt = new GlobalUpliftAttempt()
                        {
                            DateAttempted = searchResult.Date.GetValueOrDefault(dateProcessed),
                            RouteNumber = searchResult.RouteId,
                            PlannedStopNumber = searchResult.StopId,
                            CollectedQty = (short)Math.Abs(searchResult.CollectedQty),
                            DriverName = searchResult.DriverName,
                            SourceFilename = searchResult.Filename
                        };
                        attempt.GlobalUplift = globalUplift;
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
                        if (attempt.CollectedQty > 0)
                        {
                            globalUplift.CollectedQty = attempt.CollectedQty;
                        }

                        // Add to EF, cache & parent instance
                        _wellEntities.GlobalUpliftAttempt.Add(attempt);
                        _globalUpliftAttempts.Add(attempt);
                        globalUplift.GlobalUpliftAttempt.Add(attempt);
                        Console.WriteLine($"Adding uplift attempt {searchResult.BranchId:##} {searchResult.AccountNumber}");
                        counter++;

                        if (counter >= 100)
                        {
                            // We need the attempt PK ID for the ADAM event
                            _wellEntities.SaveChanges();
                            counter = 0;
                        }

                        //if (attempt.CollectedQty > 0)
                        //{
                        //    SendGlobalUpliftEmail(attempt, globalUplift);
                        //}

                        //// Tell ADAM
                        //SendGlobalUpliftAttempt(attempt, globalUplift);
                    }
                    _wellEntities.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Skipping existing uplift attempt {searchResult.BranchId:##} {searchResult.AccountNumber}");
                }
            }
        }

        #endregion private helper methods
    }
}
