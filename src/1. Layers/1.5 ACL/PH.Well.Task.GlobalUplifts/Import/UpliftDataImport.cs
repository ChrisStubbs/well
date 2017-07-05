using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Task.GlobalUplifts.Data;

namespace PH.Well.Task.GlobalUplifts.Import
{
    public class UpliftDataImport : IUpliftDataImport
    {
        private readonly IUpliftDataProvider _dataProvider;

        public UpliftDataImport(IUpliftDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Import()
        {
            throw new NotImplementedException();
            foreach (var upliftData in _dataProvider.GetUpliftData())
            {
                
            }
        }

        /// <summary>
        /// Validates data returned by IUpliftDataProvider
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValidationResult> Validate()
        {
            foreach (var upliftData in _dataProvider.GetUpliftData())
            {
                var validationResult = Validate(upliftData);
                if (upliftData != null)
                {
                    yield return validationResult;
                }
            }
        }

        /// <summary>
        /// Validates given uplift data object
        /// </summary>
        /// <param name="data">Uplift data to validate</param>
        /// <returns>Validation result or null if record is valid</returns>
        private ValidationResult Validate(IUpliftData data)
        {
            // Implement validation rules here


        }
    }
}
