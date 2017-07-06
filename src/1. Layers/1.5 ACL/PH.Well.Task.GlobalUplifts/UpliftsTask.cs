using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Task.GlobalUplifts.Csv;
using PH.Well.Task.GlobalUplifts.Data;
using PH.Well.Task.GlobalUplifts.Import;

namespace PH.Well.Task.GlobalUplifts
{
    public class UpliftsTask
    {
        private readonly IUpliftDataImportService _importService;

        public UpliftsTask(IUpliftDataImportService importService)
        {
            _importService = importService;
        }

        public void Execute(UpliftsTaskData data)
        {
            var providersCollection = new UpliftDataProvidersCollection();
            foreach (var dataDirectory in data.Directories)
            {
                providersCollection.Add(new DirectoryCsvUpliftDataProvider(dataDirectory));
            }

            //Run import
            _importService.Import(providersCollection);
        }
    }
}
