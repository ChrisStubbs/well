namespace PH.Well.Common.Security
{
    using System;
    using System.Linq;
    using NLog;

    using PH.Well.Common.Contracts;

    public class UserRoleProvider : System.Web.Security.RoleProvider, IUserRoleProvider
    {
        private const string CacheKey = "ph.orderwell";

        private readonly HttpContextCache cache;

        public override string ApplicationName { get; set; }

        public UserRoleProvider()
        {
            this.cache = new HttpContextCache();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var roles = this.GetRolesForUser(username);

            return roles.Any(role => role == roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            var cachedRoles = this.cache.Get<string[]>(CacheKey, username);

            if (cachedRoles != null && cachedRoles.Length > 0)
            {
                return cachedRoles;
            }

            var roles = GetRoles(username);

            var shouldCacheRoles = true;

            if (System.Configuration.ConfigurationManager.AppSettings["shouldCacheRoles"] != null)
            {
                shouldCacheRoles = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["ShouldCacheRoles"]);
            }

            if (shouldCacheRoles)
            {
                this.cache.AddItem(CacheKey, username, roles, 5);
            }

            return roles;
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
            string[] roles;

            try
            {
                var roleProvideHelper = new RoleProviderHelper();

                roles = roleProvideHelper.GetRoles(username);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                roles = new string[0];
            }

            return roles;
        }
    }
}
