using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.Enums
{
    public enum ChangeSource
    {
        Client = -2,
        Unknown = -1,
        Data = 0,
        Timeout = 1,
        Object = 2,
        Database = 3,
        System = 4,
        Statement = 5,
        Environment = 6,
        Execution = 7,
        Owner = 8,
    }
}
