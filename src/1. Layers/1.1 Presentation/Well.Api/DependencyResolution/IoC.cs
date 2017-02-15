namespace PH.Well.Api.DependencyResolution
{
    using System;
    using System.Threading;
    using StructureMap;
    
    public static class IoC
    {
        private static readonly Lazy<Container> _containerBuilder =
                new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get
            {
                return _containerBuilder.Value;
            }
        }

        private static Container defaultContainer()
        {
            return new Container(c => c.AddRegistry<DefaultRegistry>());
        }
    }
}