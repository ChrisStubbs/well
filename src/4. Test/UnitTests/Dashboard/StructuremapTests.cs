namespace PH.Well.UnitTests.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using global::Well.Dashboard.Controllers;
    using NUnit.Framework;

    using PH.Well.Dashboard.Controllers;

    using Repositories.Contracts;
    using StructureMap;
    using Well.Dashboard.DependencyResolution;

    [TestFixture]
    public class StructuremapTests
    {
        [Test]
        public void WhenGettingControllers_ThenAllServicesAreRegistered()
        {
            IContainer container = IoC.Initialize();
            var structureMapDependencyScope = new StructureMapDependencyScope(container);

            List<Type> controllerTypes = GetSubClasses<BaseController>();

            foreach (Type controllerType in controllerTypes)
            {
                object controller;
                Assert.DoesNotThrow(() => controller = structureMapDependencyScope.Container.GetInstance(controllerType));
            }
        }

        [Test]
        public void WhenGettingDbChangeNotifier_ThenAllServicesAreRegistered()
        {
            IContainer container = IoC.Initialize();
            var structureMapDependencyScope = new StructureMapDependencyScope(container);

            IDbChangeNotifier dbChangeNotifier;
            Assert.DoesNotThrow(
                () => dbChangeNotifier = structureMapDependencyScope.Container.GetInstance<IDbChangeNotifier>());

        }

        private static List<Type> GetSubClasses<T>()
        {
            var mvcAssembly = typeof(HomeController).Assembly;
            return mvcAssembly.GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList();
        }
    }
}
