namespace PH.Well.Dashboard.Extensions
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using Newtonsoft.Json.Linq;

    using PH.Well.Common.Contracts;
    using PH.Well.Dashboard;
    using PH.Well.Dashboard.Modules;

    public static class HtmlHelperExtensions
    {
        private static string applicationName = Configuration.ApplicationName;

        private static string applicationVersion;

        private static string apiVersion;

        private static ILogger logger;

        /// <summary>
        /// Provides the current version of the application.
        /// </summary>
        /// <param name="htmlHelper">
        /// The HtmlHelper.
        /// </param>
        /// <returns>
        /// The current version as a <see cref="string"/>.
        /// </returns>
        public static string CurrentVersion(this HtmlHelper htmlHelper)
        {
            if (applicationVersion != null)
                return applicationVersion;

            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                applicationVersion = version.ToString();
            }
            catch
            {
                applicationVersion = "unknown";
            }

            return applicationVersion;
        }

        public static string ApiVersion(this HtmlHelper htmlHelper, IWebClient webClient = null)
        {
            if (apiVersion != null)
                return apiVersion;

            try
            {
                if (webClient == null) webClient = new PH.Well.Common.WebClient { Credentials = CredentialCache.DefaultCredentials };

                var result = webClient.DownloadString($"{Configuration.OrderWellApi}{"Version"}");

                if (result != string.Empty)
                {
                    var version = JObject.Parse(result);
                    apiVersion = version.GetValue("version").ToString();
                }
            }
            catch (Exception)
            {
                apiVersion = "unknown";
            }

            return apiVersion;
        }

        /// <summary>
        /// Provides the application name.
        /// </summary>
        /// <param name="htmlHelper">
        /// The HtmlHelper.
        /// </param>
        /// <returns>
        /// The application name as a <see cref="string"/>.
        /// </returns>
        public static string ApplicationName(this HtmlHelper htmlHelper)
        {
            if (applicationName != null)
                return applicationName;

            try
            {
                var name = Assembly.GetExecutingAssembly().GetName();
                applicationName = name.ToString();
            }
            catch
            {
                applicationName = "unknown";
            }

            return applicationName;
        }

        /// <summary>
        /// Returns the user avatar
        /// </summary>
        /// <param name="htmlHelper">The HtmlHelper</param>
        /// <returns>The avatar as a <see cref="string"/>.</returns>
        public static string EmployeeAvatar(this HtmlHelper htmlHelper)
        {
            return UserModule.EmployeeAvatar(UserModule.UserName(htmlHelper.ViewContext.HttpContext.User.Identity.Name));
        }

        public static string EmployeeDisplayName(this HtmlHelper htmlHelper)
        {
            return UserModule.DisplayName(HttpContext.Current.User.Identity.Name);
        }

        public static MvcHtmlString AuthorisedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, params string[] requiredPermissions)
        {
            var hasRequiredPermission = requiredPermissions.Any(permission => HttpContext.Current.User.IsInRole(permission));
            return hasRequiredPermission ? htmlHelper.ActionLink(linkText, actionName, controllerName) : null;
        }
    }
}