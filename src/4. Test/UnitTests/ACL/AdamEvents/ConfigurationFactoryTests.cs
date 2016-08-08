namespace PH.Well.UnitTests.ACL.AdamEvents
{
    using NUnit.Framework;

    using PH.Well.Adam.Events;
    using PH.Well.Domain.Enums;

    using Config = PH.Well.Adam.Events.Configuration;

    [TestFixture]
    public class ConfigurationFactoryTests
    {
        [Test]
        public void MedwayConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.MedwayIsDown);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsMedway));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortMedway));
        }

        [Test]
        public void CoventryConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Coventry);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsCoventry));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortCoventry));
        }

        [Test]
        public void FarehamConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Fareham);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsFareham));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortFareham));
        }

        [Test]
        public void DunfermlineConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Dunfermline);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsDunfermline));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortDunfermline));
        }

        [Test]
        public void LeedsConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Leeds);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsLeeds));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortLeeds));
        }

        [Test]
        public void HemelConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Hemel);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsHemel));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortHemel));
        }

        [Test]
        public void BirtleyConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Birtley);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBirtley));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBirtley));
        }

        [Test]
        public void BelfastConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Belfast);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBelfast));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBelfast));
        }

        [Test]
        public void BrandonConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Brandon);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBrandon));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBrandon));
        }

        [Test]
        public void PlymouthConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Plymouth);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsPlymouth));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortPlymouth));
        }

        [Test]
        public void BristolConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Bristol);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsBristol));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortBristol));
        }

        [Test]
        public void HaydockConfig()
        {
            var config = ConfigurationFactory.GetAdamConfiguration(Branch.Haydock);

            Assert.That(config.Username, Is.EqualTo(Config.AdamUsername));
            Assert.That(config.Password, Is.EqualTo(Config.AdamPassword));
            Assert.That(config.Server, Is.EqualTo(Config.AdamServer));
            Assert.That(config.Rfs, Is.EqualTo(Config.AdamRfsHaydock));
            Assert.That(config.Port, Is.EqualTo(Config.AdamPortHaydock));
        }
    }
}