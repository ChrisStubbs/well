using System;
using System.Linq;
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
