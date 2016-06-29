namespace PH.Well.UnitTests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using NUnit.Framework;
    using StructureMap;
    using Well.Api.Controllers;
    using Well.Api.DependencyResolution;

    [TestFixture]
    public class StructuremapTests
    {
        [Test]
        public void WhenGettingControllers_ThenAllServicesAreRegistered()
        {
            IContainer container = IoC.Initialize();
            var structureMapDependencyScope = new StructureMapDependencyScope(container);

            List<Type> controllerTypes = GetSubClasses<ApiController>();

            foreach (Type controllerType in controllerTypes)
            {
                object controller;
                Assert.DoesNotThrow(() => controller = structureMapDependencyScope.Container.GetInstance(controllerType));
            }
        }

        private static List<Type> GetSubClasses<T>()
        {
            var mvcAssembly = typeof(VersionController).Assembly;
            return mvcAssembly.GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList();
        }
    }
}
