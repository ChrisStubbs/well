using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.Services
{
    using System.Configuration;

    public struct AdamConfiguration
    {
        #region Constants

        private static readonly char[] Delimiters = {';'};
        private const string UsernameKey = "username";
        private const string PasswordKey = "password";
        private const string ServerKey = "server";
        private const string PortKey = "port";
        private const string RfsKey = "rfs";
        private const string MissingMessage = "adam_Default setting missing from config";
        private static readonly string[] OrderedKeyNames = 
        {
            ServerKey,
            PortKey,
            RfsKey,
            UsernameKey,
            PasswordKey
        };

        #endregion Constants

        #region Private properties
        /// <summary>
        /// Used to cache a single read of the default settings - accessed only by AdamDefault public property
        /// </summary>
        private static AdamSettings adamDefault => CreateSettings(ConfigurationManager.AppSettings["adam_Default"] ?? "Missing:12345:MissingRfs");
        #endregion Private properties

        #region Public static properties
        public static readonly AdamSettings AdamDefault = adamDefault;
        public static AdamSettings AdamMedway => CreateSettings(ConfigurationManager.AppSettings["adamMedway"]);
        public static AdamSettings AdamCoventry => CreateSettings(ConfigurationManager.AppSettings["adamCoventry"]);
        public static AdamSettings AdamFareham => CreateSettings(ConfigurationManager.AppSettings["adamFareham"]);
        public static AdamSettings AdamDunfermline => CreateSettings(ConfigurationManager.AppSettings["adamDunfermline"]);
        public static AdamSettings AdamLeeds => CreateSettings(ConfigurationManager.AppSettings["adamLeeds"]);
        public static AdamSettings AdamHemel => CreateSettings(ConfigurationManager.AppSettings["adamHemel"]);
        public static AdamSettings AdamBirtley => CreateSettings(ConfigurationManager.AppSettings["adamBirtley"]);
        public static AdamSettings AdamBelfast => CreateSettings(ConfigurationManager.AppSettings["adamBelfast"]);
        public static AdamSettings AdamBrandon => CreateSettings(ConfigurationManager.AppSettings["adamBrandon"]);
        public static AdamSettings AdamPlymouth => CreateSettings(ConfigurationManager.AppSettings["adamPlymouth"]);
        public static AdamSettings AdamBristol => CreateSettings(ConfigurationManager.AppSettings["adamBristol"]);
        public static AdamSettings AdamHaydock => CreateSettings(ConfigurationManager.AppSettings["adamHaydock"]);
        #endregion Public static properties

        #region Private helper methods

        /// <summary>
        /// Create Adam Settings from a config string
        /// </summary>
        /// <param name="adamSetting">A connection string in customer format</param>
        /// <returns>An Adam configuration, or the default configuration for any missing settings</returns>
        /// <remarks>
        /// Can be in the form
        ///     value="servername:portnumber;rfsname"
        /// Or in the explicit form
        ///     value="Server=servername;Port=portnumber;Rfs=rfsname"
        /// *Note: Server, Port and Rfs are case insensitive
        /// </remarks>
        public static AdamSettings CreateSettings(string adamSetting)
        {
            var settings = (adamSetting ?? "").Split(Delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (settings.Length > 0)
            {
                Dictionary<string, string> values = new Dictionary<string, string>();
                // If the first entry has no "=", create a Key Value list from the assumed sequence
                if (!(settings.FirstOrDefault() ?? "").Contains('='))
                {
                    // Convert each parameter in sequence to a named key-value entry
                    for (int index = 0; index < OrderedKeyNames.Length && index < settings.Length; index++)
                    {
                        values.Add(OrderedKeyNames[index], settings[index].Trim());
                    }
                }
                else
                {
                    // Extract the key values from each segment
                    values = settings.Select(x =>
                    {
                        var split = x.Split('=');
                        return new { key = split[0].Trim().ToLower(), value = split[1].Trim() };
                    }).ToDictionary(x => x.key, x => x.value);
                }

                // Add any missing values from the adamDefault or global defaults
                AdamSettings adamSettings = new AdamSettings
                {
                    Username =
                        values.ContainsKey(UsernameKey) ? values[UsernameKey] : AdamDefault?.Username ?? MissingMessage,
                    Password =
                        values.ContainsKey(PasswordKey) ? values[PasswordKey] : AdamDefault?.Password ?? MissingMessage,
                    Server = 
                        values.ContainsKey(ServerKey) ? values[ServerKey] : AdamDefault?.Server ?? MissingMessage,
                    Port = 
                        values.ContainsKey(PortKey) ? int.Parse(values[PortKey]) : AdamDefault?.Port ?? 0,
                    Rfs = 
                        values.ContainsKey(RfsKey) ? values[RfsKey] : AdamDefault?.Rfs ?? MissingMessage
                };
                return adamSettings;
            }
            return AdamDefault;
        }
        #endregion Private helper methods
    }
}