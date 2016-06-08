using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.Enums
{
    public enum ReasonCategory
    {
        [Description("Auth")]
        Auth = 1,

        [Description("DA")]
        Da = 2,

        [Description("DR")]
        Dr = 3
    }
}
