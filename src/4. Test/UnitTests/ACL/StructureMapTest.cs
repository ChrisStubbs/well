namespace PH.Well.UnitTests.ACL
{
    using Adam.Events;
    using Clean;
    using NUnit.Framework;
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
            IFileMonitorService monitorService;
            Assert.DoesNotThrow(() => monitorService = container.GetInstance<IFileMonitorService>());
        }

        [Test]
        public void CheckWellCleanIoc()
        {
            var container = Clean.Program.InitIoc();
            ITriggerCleanProcess trigger;
            Assert.DoesNotThrow(() => trigger = container.GetInstance<ITriggerCleanProcess>());

        }
    }
}