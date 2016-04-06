using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Common
{
    using System.Configuration;
    using System.Web;
    using Contracts;
    using Newtonsoft.Json;
    using Security;

    public class SecurityApiClient
    {
        private IApiClient apiClient;


        public SecurityApiClient()
        {
            apiClient = new ApiClient();
        }

        public class Role
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Guid ApplicationId { get; set; }
            public string[] Permissions { get; set; }
        }

        public class User
        {
            public const string IdentityTypeToken = "Token";
            public const string IdentityActiveDirectory = "ActiveDirectory";

            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string JobTitle { get; set; }
            public string IdentityType { get; set; }
        }

        /// <summary>
        /// Uses the security api to get user details
        /// </summary>
        /// <param name="userIdentifier"> the active directory identity identifier or the token identifier</param>
        /// <returns></returns>
        public virtual User GetUser(string userIdentifier)
        {
            var userUri = ConfigurationManager.AppSettings["SecurityApi"] + "/Users?useridentifier=" + HttpUtility.UrlEncode(userIdentifier);
            var user = JsonConvert.DeserializeObject<List<User>>(apiClient.DownloadString(userUri)).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Uses the security api to get user Roles
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<Role> GetRoles(Guid userId)
        {
            var rolesUri = ConfigurationManager.AppSettings["SecurityApi"] + "/Users/" + HttpUtility.UrlEncode(userId.ToString()) + "/Roles";
            var roles = JsonConvert.DeserializeObject<List<Role>>(apiClient.DownloadString(rolesUri));
            return roles;
        }

        public virtual void AddUserToRoles(UserRoleRequest request)
        {
            var uri = ConfigurationManager.AppSettings["SecurityApi"] + "/Userroles";
            apiClient.UploadString(uri, "POST", JsonConvert.SerializeObject(request));
        }


    }
}
