using System.Collections.Generic;

namespace PH.Well.Task.GlobalUplifts
{
    public class UpliftImportTaskData
    {
        public UpliftImportTaskData()
        {
            Directories = new List<string>();
        }

        /// <summary>
        /// Import file directories
        /// </summary>
        public IList<string> Directories { get; set; }
    }
}