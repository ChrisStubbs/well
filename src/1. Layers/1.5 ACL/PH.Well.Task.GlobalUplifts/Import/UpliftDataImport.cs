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
            var dataSet = _dataProvider.GetUpliftData();

            if (dataSet.HasErrors)
            {
                //Log errors or w/e and stop the import

                return;
            }


        }
    }
}
