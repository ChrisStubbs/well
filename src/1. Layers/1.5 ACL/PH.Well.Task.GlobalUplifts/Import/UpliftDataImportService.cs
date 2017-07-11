using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Common;
using PH.Well.Common.Contracts;
using PH.Well.Domain;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Task.GlobalUplifts.Data;
using Branch = PH.Well.Domain.Enums.Branch;

namespace PH.Well.Task.GlobalUplifts.Import
{
    public class UpliftDataImportService : IUpliftDataImportService
    {
        private readonly ILogger _logger;
        private readonly IEventLogger _eventLogger;
        private readonly IAdamRepository _adamRepository;
        private readonly IRouteHeaderRepository _routeHeaderRepository;

        public UpliftDataImportService(ILogger logger,IEventLogger eventLogger,IAdamRepository adamRepository, IRouteHeaderRepository routeHeaderRepository)
        {
            _logger = logger;
            _eventLogger = eventLogger;
            _adamRepository = adamRepository;
            _routeHeaderRepository = routeHeaderRepository;
        }

        public void Import(IUpliftDataProvider dataProvider)
        {
            foreach (var dataSet in dataProvider.GetUpliftData())
            {
                if (dataSet.HasErrors)
                {
                    var exception = new ValidationException(string.Join(Environment.NewLine,
                        dataSet.Errors.SelectMany(x => new[] {x.ErrorMessage}.Concat(x.MemberNames))));

                    _logger.LogError($"Uplift import error. DataSet {dataSet.Id}", exception);
                    _eventLogger.TryWriteToEventLog(EventSource.WellGlobalUpliftTask, exception);
                    //throw exception ? Send email ?
                }
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
                    var route = _routeHeaderRepository.Create(new Routes {FileName = dataSet.Id});
                    // Process set here
                    foreach (var r in dataSet.Records)
                    {
                        var transaction = new GlobalUpliftTransaction(GenerateTransactionId(r, route.Id), r.BranchId,
                            r.AccountNumber,
                            r.CreditReasonCode,
                            r.ProductCode, r.Quantity, r.StartDate, r.EndDate);

                        //Write to adam
                        var status = _adamRepository.GlobalUplift(transaction, GetAdamSettings(r.BranchId));
                        
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
