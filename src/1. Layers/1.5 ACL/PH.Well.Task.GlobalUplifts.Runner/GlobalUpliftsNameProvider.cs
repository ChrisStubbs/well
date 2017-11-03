using PH.Well.Common.Contracts;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    public class GlobalUpliftsNameProvider : IUserNameProvider, PH.Common.Security.Interfaces.IUserNameProvider
    {
        private static string _username = "GlobalUplifts";

        public string GetUserName()
        {
            return _username;
        }

        public static void SetUsername(string username)
        {
            _username = username;
        }
    }
}
