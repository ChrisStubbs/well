using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Task.GlobalUplifts.Data;

namespace PH.Well.Task.GlobalUplifts.Import
{
    public interface IUpliftDataImportService
    {
        void Import(IUpliftDataProvider dataProvider);
    }
}
