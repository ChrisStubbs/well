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
        private readonly ILogger _logger;
        private readonly IEventLogger _eventLogger;
        private readonly IAdamRepository _adamRepository;
        private readonly IRouteHeaderRepository _routeHeaderRepository;
        private readonly WellEntities _wellEntities;

        public UpliftDataImportService(ILogger logger, IEventLogger eventLogger, IAdamRepository adamRepository, IRouteHeaderRepository routeHeaderRepository, WellEntities wellEntities)
        {
            _logger = logger;
            _eventLogger = eventLogger;
            _adamRepository = adamRepository;
            _routeHeaderRepository = routeHeaderRepository;
            _wellEntities = wellEntities;
        }

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
                // TODO: Uncomment this
                //////else if (_routeHeaderRepository.FileAlreadyLoaded(dataSet.Id))
                //////{
                //////    var exception = new ValidationException($"Uplift data already imported. Set : {dataSet.Id}");

                //////    _logger.LogError(exception.Message, exception);
                //////    _eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, exception);
                //////    //throw exception ? Send email ?
                //////}
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
                        _wellEntities.SaveChanges();

                        // Now tell Adam about the uplift
                        var transaction = new GlobalUpliftTransaction(GenerateTransactionId(record, route.Id), record.BranchId,
                            record.AccountNumber,
                            record.CreditReasonCode,
                            record.ProductCode, 
                            record.Quantity, 
                            record.StartDate, 
                            record.EndDate, 
                            0, 
                            record.CustomerReference);

                        //Write to adam
                        // TODO: remove this
                        if (false)
                        {
                            var status = _adamRepository.GlobalUplift(transaction, GetAdamSettings(record.BranchId));
                        }

                        // do we need to check status and do something after ?
                    }
                }
            }
        }

        private AdamSettings GetAdamSettings(int branchId)
        {
            return AdamSettingsFactory.GetAdamSettings((Branch)branchId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upliftData">Import record</param>
        /// <param name="routeId">Import id</param>
        /// <returns></returns>
        private int GenerateTransactionId(IUpliftData upliftData, int routeId)
        {
            var idString = $"{GlobalUpliftTransaction.WELLHDRCDTYPE}{routeId}{upliftData.Id}";
            return int.Parse(idString);
        }
    }
}
