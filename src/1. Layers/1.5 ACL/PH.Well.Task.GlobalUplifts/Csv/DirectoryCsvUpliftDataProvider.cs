using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Task.GlobalUplifts.Data;

namespace PH.Well.Task.GlobalUplifts.Csv
{
    public class DirectoryCsvUpliftDataProvider : UpliftDataProvidersCollection
    {
        #region Fields
        private readonly string _directoryPath;
        private readonly string _archivePath;
        private readonly int _maxUpliftEndDateDays;
        #endregion Fields

        #region Constructors
        public DirectoryCsvUpliftDataProvider(string directoryPath,string archivePath, int maxUpliftEndDateDays)
        {
            _directoryPath = directoryPath;
            _archivePath = archivePath;
            _maxUpliftEndDateDays = maxUpliftEndDateDays;
        }
        #endregion Constructors

        #region Public methods
        public override IEnumerable<UpliftDataSet> GetUpliftData()
        {
            // Get providers for given directory
            GetProviders();
            return base.GetUpliftData();
        }

        private void GetProviders()
        {
            foreach (var csvFile in Directory.GetFiles(_directoryPath, "*.csv"))
            {
                Add(new CsvUpliftDataProvider(csvFile, _archivePath) {MaxUpliftEndDateDays = this._maxUpliftEndDateDays});
            }
        }
        #endregion Public methods
    }
}
