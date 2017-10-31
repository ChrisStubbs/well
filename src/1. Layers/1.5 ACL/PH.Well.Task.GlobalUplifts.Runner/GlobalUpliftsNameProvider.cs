using PH.Well.Common.Contracts;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    public class GlobalUpliftsNameProvider : IUserNameProvider
    {
        public string GetUserName()
        {
            return "GlobalUplifts";
        }
    }
}