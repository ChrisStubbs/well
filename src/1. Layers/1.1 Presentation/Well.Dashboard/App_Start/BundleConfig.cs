namespace PH.Well.Dashboard
{
    using System.Web.Optimization;

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

            var cssBundle = new StyleBundle("~/bundles/css/all").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/bootstrap-responsive.min.css",
                "~/Content/css/bootstrap-flatten.css",
                "~/Content/css/toastr.min.css",
                "~/Content/css/site.css");

            cssBundle.Orderer = new DefinedBundlerOrderer();
            bundles.Add(cssBundle);
            
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                  "~/Scripts/jquery.signalR-2.2.0.min.js"));
        }
    }
}
