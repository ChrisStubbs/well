namespace PH.Well.Api.Controllers
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Web.Hosting;
    using System.Web.Http;

    public class VersionController : ApiController
    {
        private static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Used to find the current version of the Account API
        /// </summary>
        /// <returns>A <see cref="string"/> containing the the version number</returns>
        [Route("version")]
        public HttpResponseMessage Get()
        {
            var deploymentDate = File.GetLastWriteTime(Path.Combine(HostingEnvironment.MapPath("~"), "web.config"));
            return this.Request.CreateResponse(HttpStatusCode.OK, new { version = string.Format("{0} ({1})", Version, deploymentDate.ToShortDateString()) });
        }

        [Route("IsDebug")]
        [HttpGet]
        public HttpResponseMessage IsDebug()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, false); 

                bool isDebugMode = false;
            #if DEBUG
                isDebugMode = true;
            #endif

            return this.Request.CreateResponse(HttpStatusCode.OK, isDebugMode);
        }
    }
}
