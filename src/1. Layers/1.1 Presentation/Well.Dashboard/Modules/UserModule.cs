namespace PH.Well.Dashboard.Modules
{
    using System;
    using System.Globalization;

    public static class UserModule
    {
        /// <summary>
        /// Formats Active Directory user name for display
        /// </summary>
        /// <param name="name">The username to format.</param>
        /// <returns>The user's name as a <see cref="string"/>.</returns>
        public static string DisplayName(string name)
        {
            var textInfo = new CultureInfo("en-GB", false).TextInfo;
            return textInfo.ToTitleCase(name.Replace(@"PALMERHARVEY\", string.Empty).Replace(".", " "));
        }

        /// <summary>
        /// Formats user name for display
        /// </summary>
        /// <param name="name">
        /// The username to format.
        /// </param>
        /// <returns>
        /// The user's name as a <see cref="string"/>.
        /// </returns>
        public static string UserName(string name)
        {
            var textInfo = new CultureInfo("en-GB", false).TextInfo;
            return textInfo.ToTitleCase(name.Replace(@"PALMERHARVEY\", string.Empty));
        }

        /// <summary>
        /// Displays the user's avatar
        /// </summary>
        /// <param name="name">User name</param>
        /// <returns>The user's name as a <see cref="string"/>.</returns>
        public static string EmployeeAvatar(string name)
        {
            return new Uri($@"{Configuration.SecurityApi}/images?userIdentifier={name.ToLowerInvariant()}").ToString();
        }
    }
}