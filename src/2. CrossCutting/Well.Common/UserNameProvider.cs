using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PH.Well.Common.Contracts;

namespace PH.Well.Common
{
    public class UserNameProvider : IUserNameProvider
    {
        public string GetUserName()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }

        public string ChangeUserName(string userName)
        {
            var lastName = GetUserName();
            var identity = new GenericIdentity(userName);
            var principal = new GenericPrincipal(identity, new string[] {});
            Thread.CurrentPrincipal = principal;
            return lastName;
        }
    }
}
