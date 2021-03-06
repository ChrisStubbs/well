﻿namespace PH.Well.UnitTests.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    using Repositories.Contracts;
    using StructureMap;

    [TestFixture]
    public class StructuremapTests
    {
        // Commented out as the reference to the dashboard breaks team city
        // as the typescript files are compiled breaking octopus due to duplicate javascript files being generated by typescript
        /*[Test]
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
        }*/
    }
}
