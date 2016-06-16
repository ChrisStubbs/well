using System.Web;
using System.Web.Optimization;

namespace Well.Dashboard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsscripts").Include(
                  "~/Scripts/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/bootstrap-responsive.css",
                      "~/Content/css/bootstrap-flatten.css",
                      "~/Content/css/toastr.min.css",
                      "~/Content/css/jquery-ui.min.css",
                      "~/Content/css/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular2").Include(
                        "~/Scripts/angular2/es6-shim.js",
                        "~/Scripts/angular2/system-polyfills.js",
                        "~/Scripts/angular2/angular2-polyfills.js",
                        "~/Scripts/angular2/system.src.js",
                        "~/Scripts/angular2/rx.js",
                        "~/Scripts/angular2/angular2.dev.js",
                        "~/Scripts/angular2/router.dev.js",
                        "~/Scripts/angular2/http.dev.js"
                        ));
        }
    }
}
