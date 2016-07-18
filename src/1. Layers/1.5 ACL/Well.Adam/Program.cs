namespace PH.Well.Adam
{
    using Infrastructure;
    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    using StructureMap;

    public class Program
    {
        static void Main(string[] args)
        {
            var container = DependancyRegister.InitIoc();
            new Import().Process(container);
        }

    }
}
