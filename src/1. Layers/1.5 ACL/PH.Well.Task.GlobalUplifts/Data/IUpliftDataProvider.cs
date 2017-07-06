using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts.Data
{
    public interface IUpliftDataProvider
    {
        IEnumerable<UpliftDataSet> GetUpliftData();
    }
}
