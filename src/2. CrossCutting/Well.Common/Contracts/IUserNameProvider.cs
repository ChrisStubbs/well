using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Common.Contracts
{
    public interface IUserNameProvider
    {
        /// <summary>
        /// Return the current username
        /// </summary>
        /// <returns></returns>
        string GetUserName();

        /// <summary>
        /// Change the current username for subsequent GetUserName calls
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Returns the previous identity name</returns>
        string ChangeUserName(string userName);
    }
}
