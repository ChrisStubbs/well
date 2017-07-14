using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PH.Well.Services;

namespace PH.Well.UnitTests.Services
{
    [TestFixture]
    class DateThresholdServiceTests
    {
        [Test]
        public void ShouldAddTwoDays()
        {
            var sut = new DateThresholdService();
            var currentDate = DateTime.Now.Date;
            var expectedDate = DateTime.Now.Date.AddDays(2);

            Assert.That(sut.EarliestSubmitDate(currentDate, 0), Is.EqualTo(expectedDate));

        }
    }
}
