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
