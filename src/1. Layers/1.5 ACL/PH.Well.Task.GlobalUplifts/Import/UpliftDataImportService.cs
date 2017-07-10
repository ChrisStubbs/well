using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Common.Contracts;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Task.GlobalUplifts.Data;

namespace PH.Well.Task.GlobalUplifts.Import
{
    public class UpliftDataImportService : IUpliftDataImportService
    {
        private readonly ILogger _logger;
        private readonly IAdamRepository _adamRepository;

        public UpliftDataImportService(ILogger logger,IAdamRepository adamRepository)
        {
            _logger = logger;
            _adamRepository = adamRepository;
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
                    //Log errors or w/e and stop the import
                    //throw exception;
                }

                // Process set here
                foreach (var r in dataSet.Records)
                {
                    //Todo insert set as record to Routes table get id and construct transaction id
               
                    var adamSettings = GetAdamSettings(r.BranchId);
                    var transaction = new GlobalUpliftTransaction(r.BranchId, r.AccountNumber, r.CreditReasonCode,
                        r.ProductCode, r.Quantity, r.StartDate, r.EndDate);

                    //Write to adam
                    _adamRepository.GlobalUplift(transaction, adamSettings);
                }
               
            }
        }

        private AdamSettings GetAdamSettings(int branchId)
        {
            return AdamSettingsFactory.GetAdamSettings((Branch)branchId);
        }
    }
}
