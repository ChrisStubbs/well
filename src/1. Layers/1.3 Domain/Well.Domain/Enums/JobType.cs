using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.Enums
{
    public enum JobType
    {
        [Description("Auth")]
        ADHOC = 1,

        [Description("DA")]
        Da = 2,

        [Description("DR")]
        Dr = 3

    }
}
