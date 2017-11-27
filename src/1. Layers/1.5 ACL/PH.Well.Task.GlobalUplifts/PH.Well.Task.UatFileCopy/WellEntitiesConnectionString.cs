using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.Well.Data.EF.Contracts;

namespace PH.Well.Task.UatFileCopy
{
    public class WellEntitiesConnectionString : IWellEntitiesConnectionString
    {
        public WellEntitiesConnectionString(string connection)
        {
            this.NameOrConnectionString = connection;
        }

        public string NameOrConnectionString { get; set; }
    }
}
