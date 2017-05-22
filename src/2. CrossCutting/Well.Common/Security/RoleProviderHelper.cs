namespace PH.Well.Common.Security
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class RoleProviderHelper : SecurityApiClient
    {
        public string[] GetRoles(string userName)
        {
            var roles = new string[0];

            var user = this.GetUser(userName);
            if (user != null)
            {
                var applicationRoles = this.GetRoles(user.Id) ?? new List<Role>();
                var allPermissions = this.GetAllDistinctPermissionsForApplication(Guid.Parse(ConfigurationManager.AppSettings["ApplicationId"]), applicationRoles);

                if (allPermissions != null)
                {
                    roles = allPermissions;
                }
            }

            return roles;
        }

        public virtual string[] GetAllDistinctPermissionsForApplication(Guid applicationId, List<Role> allRoles)
        {
            var allPermissions = allRoles.Where(x => x.ApplicationId == applicationId).SelectMany(x => x.Permissions).Distinct().ToArray();
            return allPermissions;
        }
    }
}
