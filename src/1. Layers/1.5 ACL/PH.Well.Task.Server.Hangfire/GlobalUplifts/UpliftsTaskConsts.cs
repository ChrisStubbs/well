using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.Server.Hangfire.GlobalUplifts
{
    public static class UpliftsTaskConsts
    {
        /// <summary>
        /// This class contains configuration keys
        /// </summary>
        public static class Config
        {
            public const string DataDirectories = "GlobalUpliftsTask.DataDirectories";
            public const string MinutesInterval = "GlobalUpliftsTask.MinutesInterval";
        }

        public static class TaskNames
        {
            public const  string UpliftsTask = "GlobalUpliftsTask.GlobalUplift";
        }
    }
}
