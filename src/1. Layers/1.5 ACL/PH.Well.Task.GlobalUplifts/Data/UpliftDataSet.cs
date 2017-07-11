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
        public UpliftDataSet(string id, IEnumerable<IUpliftData> records, IEnumerable<ValidationResult> errors)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
            Records = records;
            Errors = errors;
        }

        public UpliftDataSet(string id, IEnumerable<IUpliftData> records) : this(id,records, new ValidationResult[0])
        {
        }

        /// <summary>
        /// Data set id
        /// </summary>
        public string Id { get; }
        public IEnumerable<IUpliftData> Records { get; }
        public IEnumerable<ValidationResult> Errors { get; }
        public bool HasErrors => Errors.Any();
    }
}
