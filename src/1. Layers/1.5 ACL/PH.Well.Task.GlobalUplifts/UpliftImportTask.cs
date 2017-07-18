using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Task.GlobalUplifts.Csv;
using PH.Well.Task.GlobalUplifts.Data;
using PH.Well.Task.GlobalUplifts.Import;

namespace PH.Well.Task.GlobalUplifts
{
    public class UpliftImportTask
    {
        private readonly IUpliftDataImportService _importService;

        public UpliftImportTask(IUpliftDataImportService importService)
        {
            _importService = importService;
        }

        public void Execute(UpliftImportTaskData data)
        {
            var archiveDirectoryPath = Path.Combine(data.ArchiveDirectory, DateTime.Now.ToString("yyyyMMdd"));
            Directory.CreateDirectory(archiveDirectoryPath);

            var providersCollection = new UpliftDataProvidersCollection();
            foreach (var dataDirectory in data.Directories)
            {
                providersCollection.Add(new DirectoryCsvUpliftDataProvider(dataDirectory, archiveDirectoryPath));
            }

            //Run import
            _importService.Import(providersCollection);
        }
    }
}
