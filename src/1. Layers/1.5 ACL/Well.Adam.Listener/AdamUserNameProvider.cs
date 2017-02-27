using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Common.Contracts;

namespace PH.Well.Adam.Listener
{
    public class AdamUserNameProvider: IUserNameProvider
    {
        public string GetUserName()
        {
            return "AdamInsertUpdate";
        }
    }
}
