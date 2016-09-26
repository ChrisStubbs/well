﻿namespace PH.Well.Api.Controllers
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Web.Hosting;
    using System.Web.Http;
    using Models;

    using PH.Well.Common.Contracts;
    using PH.Well.Repositories.Contracts;

    public class GlobalSettingsController : BaseApiController
    {
        private static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private readonly IUserRepository userRepository;

        private readonly IUserRoleProvider userRoleProvider;

        public GlobalSettingsController(IUserRepository userRepository, IUserRoleProvider userRoleProvider)
        {
            this.userRepository = userRepository;
            this.userRoleProvider = userRoleProvider;
        }

        [Route("global-settings")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var deploymentDate = File.GetLastWriteTime(Path.Combine(HostingEnvironment.MapPath("~"), "web.config"));
            string version = $"{Version} ({deploymentDate.ToShortDateString()})";
            var settings = new GlobalSettingsModel()
            {
                Version = version,
                IdentityName = UserIdentityName,
                UserName = userRepository.GetByIdentity(this.UserIdentityName)?.Name,
                Permissions = this.userRoleProvider.GetRolesForUser(this.UserIdentityName)
            };

            return this.Request.CreateResponse(HttpStatusCode.OK, settings);
        }
    }
}