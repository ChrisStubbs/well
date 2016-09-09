namespace PH.Well.Common.Contracts
{
    public interface IUserRoleProvider
    {
        string[] GetRolesForUser(string username);
    }
}