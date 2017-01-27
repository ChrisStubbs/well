namespace PH.Well.UnitTests.ACL.AdamEvents
{
    using NUnit.Framework;

    [TestFixture]
    public class StructureMapTest
    {
        [Test]
        public void CheckIoc()
        {
            PH.Well.Adam.Events.Program.InitIoc();
        }
    }
}