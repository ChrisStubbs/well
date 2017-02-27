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
    }
}
