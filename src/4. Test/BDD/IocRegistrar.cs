namespace PH.Well.BDD
{
    using PH.Well.Common;
    using PH.Well.Common.Contracts;

    using StructureMap;

    public class IocRegistrar
    {
        private static IContainer InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<ILogger>().Use<NLogger>();
                });
        }
    }
}