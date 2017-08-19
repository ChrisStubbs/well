using System.Collections.Generic;
using System.Linq;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.UnitTests.ACL.AdamEvents
{
    using System;
    using NUnit.Framework;

    using PH.Well.Domain.Enums;
    using PH.Well.Services;

    using Config = PH.Well.Services.AdamConfiguration;

    [TestFixture]
    public class AdamSettingsFactoryTests
    {
        private void TestSettings(string settings, string server, int port, string rfs, string username, string password)
        {
            AdamSettings adamSettings = AdamConfiguration.CreateSettings(settings);
            Assert.That(adamSettings.Server, Is.EqualTo(server));
            Assert.That(adamSettings.Port, Is.EqualTo(port));
            Assert.That(adamSettings.Rfs, Is.EqualTo(rfs));
            Assert.That(adamSettings.Username, Is.EqualTo(username));
            Assert.That(adamSettings.Password, Is.EqualTo(password));
        }

        [Test]
        public void BlankConfigSetting()
        {
            AdamSettings defaultSettings = AdamSettingsFactory.GetAdamSettings(Branch.Default);
            TestSettings("", defaultSettings.Server, defaultSettings.Port, defaultSettings.Rfs, defaultSettings.Username, defaultSettings.Password);
        }

        [Test]
        public void OrderedConfigSettingParsedInOrder()
        {
            AdamSettings defaultSettings = AdamSettingsFactory.GetAdamSettings(Branch.Default);
            TestSettings("servername", "servername", defaultSettings.Port, defaultSettings.Rfs, defaultSettings.Username, defaultSettings.Password);
            TestSettings("servername:1234", "servername", 1234, defaultSettings.Rfs, defaultSettings.Username, defaultSettings.Password);
            TestSettings("servername:1234;rfspath", "servername", 1234, "rfspath", defaultSettings.Username, defaultSettings.Password);
            TestSettings("servername:1234:rfspath,username", "servername", 1234, "rfspath", "username", defaultSettings.Password);
            TestSettings("servername:1234:rfspath,username,pa$$word", "servername", 1234, "rfspath", "username", "pa$$word");
        }

        [Test]
        public void NamedConfigSettingIdentifiedSeparately()
        {
            AdamSettings defaults = AdamSettingsFactory.GetAdamSettings(Branch.Default);
            // Test each parameter separately
            TestSettings("Server=servername", "servername", defaults.Port, defaults.Rfs, defaults.Username, defaults.Password);
            TestSettings("Port=1234", defaults.Server, 1234, defaults.Rfs, defaults.Username, defaults.Password);
            TestSettings("Rfs=rfspath", defaults.Server, defaults.Port, "rfspath", defaults.Username, defaults.Password);
            TestSettings("Username=userName", defaults.Server, defaults.Port, defaults.Rfs, "userName", defaults.Password);
            TestSettings("Password=pa$$word", defaults.Server, defaults.Port, defaults.Rfs, defaults.Username, "pa$$word");

            // Test in progressively complex groups of settings in non-sequential order
            TestSettings("Port=1234,Server=servername", "servername", 1234, defaults.Rfs, defaults.Username, defaults.Password);
            TestSettings("Rfs=rfspath;Server=servername:Port=1234;", "servername", 1234, "rfspath", defaults.Username, defaults.Password);
            TestSettings("Server=servername:Port=1234:Rfs=rfspath,Username=username", "servername", 1234, "rfspath", "username", defaults.Password);
            TestSettings("Password=pa$$word;Server=servername:Port=1234:Username=username,Rfs=rfspath", "servername", 1234, "rfspath", "username", "pa$$word");

        }

        [Test]
        public void KeyNamesAreCaseInsensitive()
        {
            // Test case-insensitivity of key names
            TestSettings("PaSsWoRd=pa$$word;SeRveR=servername:PoRt=1234:UsErNaMe=username,RfS=rfspath", "servername", 1234, "rfspath", "username", "pa$$word");
        }

        [Test]
        public void KeysAndValuesAreTrimmed()
        {
            // Test whitespace is removed from values and key names
            TestSettings("   Password = pa$$word ; Server = servername : Port = 1234 : Username = username, Rfs  =  rfspath ", "servername", 1234, "rfspath", "username", "pa$$word");
        }

        [Test]
        public void ValuesCanContainSpecialCharacters()
        {
            // Test values can contain likely special characters
            TestSettings("Password=pa$$w!#£&*()[]~/\\;Server=servername.palmerharvey.co.uk:Port=1234:Username=first.last,Rfs=rfspath",
                "servername.palmerharvey.co.uk", 
                1234, 
                "rfspath", 
                "first.last",
                "pa$$w!#£&*()[]~/\\");
        }

        [Test]
        public void TestCorrectBranchConfigIsReturnedFromFactoryPerBranch()
        {
            foreach (Branch branch in Enum.GetValues(typeof(Branch)))
            {
                AdamSettings config = null;
                if ((int)branch <= (int)Branch.LastBranch)
                {
                    config = AdamSettingsFactory.GetAdamSettings(branch);
                }
                AdamSettings match = null;
                switch (branch)
                {
                    case Branch.Medway:
                        match = AdamConfiguration.AdamMedway;
                        break;
                    case Branch.Coventry:
                        match = AdamConfiguration.AdamCoventry;
                        break;
                    case Branch.Fareham:
                        match = AdamConfiguration.AdamFareham;
                        break;
                    case Branch.Dunfermline:
                        match = AdamConfiguration.AdamDunfermline;
                        break;
                    case Branch.Leeds:
                        match = AdamConfiguration.AdamLeeds;
                        break;
                    case Branch.Hemel:
                        match = AdamConfiguration.AdamHemel;
                        break;
                    case Branch.Birtley:
                        match = AdamConfiguration.AdamBirtley;
                        break;
                    case Branch.Belfast:
                        match = AdamConfiguration.AdamBelfast;
                        break;
                    case Branch.Brandon:
                        match = AdamConfiguration.AdamBrandon;
                        break;
                    case Branch.Plymouth:
                        match = AdamConfiguration.AdamPlymouth;
                        break;
                    case Branch.Bristol:
                        match = AdamConfiguration.AdamBristol;
                        break;
                    case Branch.Haydock:
                        match = AdamConfiguration.AdamHaydock;
                        break;
                    case Branch.Default:
                        match = AdamConfiguration.AdamDefault;
                        break;
                    case Branch.NotDefined:
                        match = null;
                        break;
                    default:
                        throw new ArgumentException("Branch not expected");
                }
                if (config != null && match != null)
                {
                    Assert.That(config.Username, Is.EqualTo(match.Username));
                    Assert.That(config.Password, Is.EqualTo(match.Password));
                    Assert.That(config.Server, Is.EqualTo(match.Server));
                    Assert.That(config.Rfs, Is.EqualTo(match.Rfs));
                    Assert.That(config.Port, Is.EqualTo(match.Port));
                }
            }
        }

        [Test]
        public void NotValidValue()
        {
            Assert.Throws<ArgumentException>(() => AdamSettingsFactory.GetAdamSettings((Branch)20000));
        }
    }
}