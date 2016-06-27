namespace PH.Well.Adam
{
    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    using StructureMap;

    public class Program
    {
        static void Main(string[] args)
        {
            var container = InitIoc();
            new Import().Process(container);
        }

        private static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                });
        }

    }
}
