using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.TranSend.Extensions
{
    using System.Security.Principal;

    public static class IPrincipleExtensions
    {
        public static string GetUsername(this IPrincipal user)
        {
            return user.Identity.Name.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        }
    }
}
