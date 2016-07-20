namespace PH.Well.UnitTests.Common
{
    using System;
    using NUnit.Framework;
    using Well.Common.Extensions;

    [TestFixture]
    public class StringExtensionTests
    {
        public class ToDashboardDateFormatMethod : StringExtensionTests
        {
            [Test]
            public void ShouldReturnTheCorrectDateFormat()
            {
                var dt = new DateTime(2016, 11, 26, 13, 47, 32);
                Assert.That(dt.ToDashboardDateFormat(),Is.EqualTo("26-11-2016 13:47:32") );
            }
        }
    }
}
