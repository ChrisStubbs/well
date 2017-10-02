namespace PH.Well.Common.Security
{
    using System;
    using System.Linq;
    using NLog;

    using PH.Well.Common.Contracts;

    public class UserRoleProvider : System.Web.Security.RoleProvider, IUserRoleProvider
    {

        public override string ApplicationName { get; set; }

        public override bool IsUserInRole(string username, string roleName)
        {
            return this.GetRolesForUser(username).Any(role => role == roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            return GetRoles(username);
        }

        #region methods not in use

        public override void CreateRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new System.NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new System.NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private static string[] GetRoles(string username)
        {
            try
            {
                var roleProvideHelper = new RoleProviderHelper();

                return roleProvideHelper.GetRoles(username);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                return new string[0];
            }
        }
    }
}
