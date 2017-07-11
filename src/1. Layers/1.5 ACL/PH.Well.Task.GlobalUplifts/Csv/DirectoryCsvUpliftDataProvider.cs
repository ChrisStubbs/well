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
        private readonly string _directoryPath;
        private readonly string _archivePath;

        public DirectoryCsvUpliftDataProvider(string directoryPath,string archivePath)
        {
            _directoryPath = directoryPath;
            _archivePath = archivePath;
        }

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
                Add(new CsvUpliftDataProvider(csvFile, _archivePath));
            }
        }
    }
}
