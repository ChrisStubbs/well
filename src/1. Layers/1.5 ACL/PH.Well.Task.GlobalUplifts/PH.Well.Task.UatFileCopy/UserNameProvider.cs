using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Common.Security.Interfaces;

namespace PH.Well.Task.UatFileCopy
{
    public class UserNameProvider : IUserNameProvider
    {
        public string GetUserName()
        {
            return "UatFileCopy";
        }
    }
}
