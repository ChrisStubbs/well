using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Task.GlobalUplifts.Data;

namespace PH.Well.Task.GlobalUplifts.Import
{
    public class UpliftDataImportService : IUpliftDataImportService
    {
        public void Import(IUpliftDataProvider dataProvider)
        {
            foreach (var dataSet in dataProvider.GetUpliftData())
            {
                if (dataSet.HasErrors)
                {
                    //Log errors or w/e and stop the import
                    throw new ValidationException(string.Join(Environment.NewLine,
                        dataSet.Errors.SelectMany(x => new[] { x.ErrorMessage }.Concat(x.MemberNames))));
                }

                // Process set here
            }
        }
    }
}
