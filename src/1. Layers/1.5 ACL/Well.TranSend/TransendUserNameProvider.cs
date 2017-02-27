using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Common.Contracts;

namespace PH.Well.TranSend
{
    public class TranSendUserNameProvider: IUserNameProvider
    {
        public string GetUserName()
        {
            return "TransendImport";
        }
    }
}
