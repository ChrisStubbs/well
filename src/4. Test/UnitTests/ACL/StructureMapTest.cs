namespace PH.Well.UnitTests.ACL
{
    using Adam.Events;
    using Clean;
    using NUnit.Framework;
    using TranSend;
    using TranSend.Infrastructure;
    using Well.Services.Contracts;

    [TestFixture]
    public class StructureMapTest
    {
        [Test]
        public void CheckAdamEventsIoc()
        {
            var container = PH.Well.Adam.Events.Program.InitIoc();
            IEventProcessor fdsImporter;
            Assert.DoesNotThrow(() => fdsImporter = container.GetInstance<IEventProcessor>());
        }

        [Test]
        public void CheckAdamListenerIoc()
        {
            var container = PH.Well.Adam.Listener.Program.InitIoc();
            IAdamFileMonitorService monitorService;
            Assert.DoesNotThrow(() => monitorService = container.GetInstance<IAdamFileMonitorService>());
        }

        [Test]
        public void CheckWellCleanIoc()
        {
            var container = PH.Well.Clean.Program.InitIoc();
            ICleanWell cleanWell;
            Assert.DoesNotThrow(() => cleanWell = container.GetInstance<ICleanWell>());
        }

        [Test]
        public void CheckWellTransendIoc()
        {
            var container = DependancyRegister.InitIoc();
            ITransendImport import;
            Assert.DoesNotThrow(() => import = container.GetInstance<ITransendImport>());

        }
    }
}