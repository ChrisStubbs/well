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
        [Test]
        public void MedwayConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Medway);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerMedway));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsMedway));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortMedway));
        }

        [Test]
        public void CoventryConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Coventry);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerCoventry));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsCoventry));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortCoventry));
        }

        [Test]
        public void FarehamConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Fareham);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerFareham));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsFareham));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortFareham));
        }

        [Test]
        public void DunfermlineConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Dunfermline);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerDunfermline));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsDunfermline));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortDunfermline));
        }

        [Test]
        public void LeedsConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Leeds);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerLeeds));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsLeeds));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortLeeds));
        }

        [Test]
        public void HemelConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Hemel);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerHemel));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsHemel));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortHemel));
        }

        [Test]
        public void BirtleyConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Birtley);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerBirtley));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBirtley));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBirtley));
        }

        [Test]
        public void BelfastConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Belfast);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerBelfast));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBelfast));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBelfast));
        }

        [Test]
        public void BrandonConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Brandon);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerBrandon));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBrandon));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBrandon));
        }

        [Test]
        public void PlymouthConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Plymouth);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerPlymouth));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsPlymouth));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortPlymouth));
        }

        [Test]
        public void BristolConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Bristol);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerBristol));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBristol));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBristol));
        }

        [Test]
        public void HaydockConfig()
        {
            var config = AdamSettingsFactory.GetAdamSettings(Branch.Haydock);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServerHaydock));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsHaydock));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortHaydock));
        }

        [Test]
        public void NotValidValue()
        {
            Assert.Throws<ArgumentException>(() => AdamSettingsFactory.GetAdamSettings((Branch)20000));
        }
    }
}