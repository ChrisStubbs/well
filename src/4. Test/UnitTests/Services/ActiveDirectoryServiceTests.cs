namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using PH.Well.Services;

    [TestFixture]
    public class ActiveDirectoryServiceTests
    {
        [Test]
        public void ShouldReturnUsersThatMatchAGivenName()
        {
            var service = new ActiveDirectoryService();

            var users = service.FindUsers("csprdservice", "palmerharvey").ToList();

            Assert.That(users.Count, Is.EqualTo(1));
            Assert.That(string.Compare("palmerharvey\\csprdservice", users[0].Name, StringComparison.CurrentCultureIgnoreCase), Is.EqualTo(0));
        }
    }
}