namespace PH.Well.Api.Controllers
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        // GET: Home
        public ActionResult Index()
        {
            var mode = Debugger.IsAttached ? "Debug" : "Release";

            var deploymentDate = System.IO.File.GetLastWriteTime(Path.Combine(HostingEnvironment.MapPath("~"), "web.config"));
            return this.Content(string.Concat(Assembly.GetExecutingAssembly().GetName().Name + " - ",
                $"{Version} ({deploymentDate}) ({mode})"));
        }
    }
}
