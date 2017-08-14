namespace PH.Well.Clean
{
    using Common.Contracts;
    public class WellCleanUserNameProvider: IUserNameProvider
    {
        public string GetUserName()
        {
            return "WellClean";
        }
    }
}