namespace PH.Well.Common
{
    using System.Security.Principal;
    using System.Threading;

    using PH.Well.Common.Contracts;

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
