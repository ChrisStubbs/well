using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts.Data
{
    public sealed class UpliftDataSet
    {
        public UpliftDataSet(IEnumerable<IUpliftData> records, IEnumerable<ValidationResult> errors)
        {
            Records = records;
            Errors = errors;
        }

        public UpliftDataSet(IEnumerable<IUpliftData> records) : this(records, new ValidationResult[0])
        {
        }

        public IEnumerable<IUpliftData> Records { get; }
        public IEnumerable<ValidationResult> Errors { get; }
        public bool HasErrors => Errors.Any();
    }
}
