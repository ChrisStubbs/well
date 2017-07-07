using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Task.GlobalUplifts.Csv;
using PH.Well.Task.GlobalUplifts.Import;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var directoryPath = "";
            var upliftService = new UpliftDataImportService();
            var task = new UpliftImportTask(upliftService);
            task.Execute(new UpliftImportTaskData {Directories = new[] {directoryPath}.ToList()});
        }
    }
}
