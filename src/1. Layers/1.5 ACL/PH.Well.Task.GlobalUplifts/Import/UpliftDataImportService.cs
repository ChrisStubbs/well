using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.Well.Data.EF;
using PH.Well.Common;
using PH.Well.Common.Contracts;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Task.GlobalUplifts.Data;
using Branch = PH.Well.Domain.Enums.Branch;
using Routes = PH.Well.Domain.Routes;

namespace PH.Well.Task.GlobalUplifts.Import
{
    public class UpliftDataImportService : IUpliftDataImportService
    {
        #region Constants
        private const int ONE_MILLION = 1000000;
        #endregion Constants

        #region Private fields
        private readonly ILogger _logger;
        private readonly IEventLogger _eventLogger;
        private readonly IAdamRepository _adamRepository;
        private readonly IRouteHeaderRepository _routeHeaderRepository;
        private readonly WellEntities _wellEntities;
        #endregion Private fields

        #region Constructors
        public UpliftDataImportService(ILogger logger, IEventLogger eventLogger, IAdamRepository adamRepository, IRouteHeaderRepository routeHeaderRepository, WellEntities wellEntities)
        {
            _logger = logger;
            _eventLogger = eventLogger;
            _adamRepository = adamRepository;
            _routeHeaderRepository = routeHeaderRepository;
            _wellEntities = wellEntities;
        }
        #endregion Constructors

        #region Public methods
        public void Import(IUpliftDataProvider dataProvider)
        {
            foreach (var dataSet in dataProvider.GetUpliftData())
            {
                if (dataSet.HasErrors)
                {
                    var exception = new ValidationException(string.Join(Environment.NewLine,
                        dataSet.Errors.SelectMany(x => new[] { x.ErrorMessage }.Concat(x.MemberNames))));

                    _logger.LogError($"Uplift import error. DataSet {dataSet.Id}", exception);
                    _eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, exception);
                    //throw exception ? Send email ?
                }
                // TODO: DIJ Comment this out to test repeating files
                else if (_routeHeaderRepository.FileAlreadyLoaded(dataSet.Id))
                {
                    var exception = new ValidationException($"Uplift data already imported. Set : {dataSet.Id}");

                    _logger.LogError(exception.Message, exception);
                    _eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, exception);
                    //throw exception ? Send email ?
                }
                else
                {
                    // Create record in Routes table so we can check whether this set was processed or not
                    var route = _routeHeaderRepository.Create(new Routes { FileName = dataSet.Id });
                    // Process set here
                    foreach (var record in dataSet.Records)
                    {
                        // Write to Well GlobalUplift table if not already present
                        var globalUplift = _wellEntities.GlobalUplift.FirstOrDefault(
                            x => x.BranchId == record.BranchId && x.PHAccount == record.AccountNumber &&
                                 (x.StartDate == null || x.StartDate == record.StartDate) &&
                                 (x.EndDate == null || x.EndDate == record.EndDate));

                        if (globalUplift == null)
                        {
                            globalUplift = new GlobalUplift()
                            {
                                BranchId = record.BranchId,
                                PHAccount = record.AccountNumber,
                                DateCreated = DateTime.Now,
                                SourceFilename = dataSet.Id
                            };
                            _wellEntities.GlobalUplift.Add(globalUplift);
                        }
                        else
                        {
                            int dupe = 1;
                        }

                        if (string.IsNullOrEmpty(globalUplift.SourceFilename))
                        {
                            globalUplift.SourceFilename = dataSet.Id;
                        }
                        // Update existing fields where missing
                        if (globalUplift.ExpectedQty == 0)
                        {
                            globalUplift.ExpectedQty = (short) record.Quantity;
                        }
                        if (string.IsNullOrEmpty(globalUplift.PHProductCode))
                        {
                            globalUplift.PHProductCode = record.ProductCode.ToString();
                        }
                        if (!globalUplift.StartDate.HasValue)
                        {
                            globalUplift.StartDate = record.StartDate;
                        }
                        if (!globalUplift.EndDate.HasValue)
                        {
                            globalUplift.EndDate = record.EndDate;
                        }
                        if (string.IsNullOrEmpty(globalUplift.CustomerReference))
                        {
                            globalUplift.CustomerReference = record.CustomerReference;
                        }

                        try
                        {
                            // Save Global Uplift changes before ADAM event sending (allows failure and retry)
                            _wellEntities.SaveChanges();

                            // Only send ADAM events once
                            if (!globalUplift.DateSentToAdam.HasValue)
                            {
                                // If not marked as sent, send it
                                globalUplift.DateSentToAdam = DateTime.Now;
                                _wellEntities.SaveChanges();

                                // Now tell Adam about the uplift
                                var transaction = new GlobalUpliftTransaction(GenerateTransactionId(record, globalUplift.Id),
                                    record.BranchId,
                                    record.AccountNumber,
                                    record.CreditReasonCode,
                                    record.ProductCode,
                                    record.Quantity,
                                    record.StartDate,
                                    record.EndDate,
                                    0,
                                    record.CustomerReference);

                                try
                                {
                                    // Write to adam
                                    //var status = _adamRepository.GlobalUplift(transaction, GetAdamSettings(record.BranchId));
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError($"Uplift ADAM.", e);
                                    _eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, e);
                                    Console.WriteLine(e);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError($"Uplift write failure.", e);
                            _eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, e);
                            Console.WriteLine(e);
                        }
                    }
                }
            }
        }
        #endregion Public methods

        #region Private helper methods
        /// <summary>
        /// Get the settings for a specified branch number
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        private AdamSettings GetAdamSettings(int branchId)
        {
            return AdamSettingsFactory.GetAdamSettings((Branch)branchId);
        }

        /// <summary>
        /// Create an ADAM unique ID using "10" + the Global Uplift counter ID
        /// </summary>
        /// <param name="upliftData">Now Unused - Import record</param>
        /// <param name="globalUpliftId">Global Uplift Record ID</param>
        /// <returns></returns>
        private int GenerateTransactionId(IUpliftData upliftData, int globalUpliftId)
        {
            try
            {
                var idString = $"{GlobalUpliftTransaction.WELLHDRCDTYPE}{globalUpliftId % ONE_MILLION}";
                return int.Parse(idString);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating ADAM ID", ex);
                _eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, ex);
                return globalUpliftId;
            }
        }
        #endregion Private helper methods
    }
}
