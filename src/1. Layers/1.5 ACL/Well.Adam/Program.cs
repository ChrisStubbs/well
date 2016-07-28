namespace PH.Well.Adam
{
    using System.Collections.Generic;
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
            string adamStatusMessage = string.Empty; 
            var container = DependancyRegister.InitIoc();
            var logger = container.GetInstance<ILogger>();
            new Import().Process(container, ref adamStatusMessage);
            logger.LogDebug(adamStatusMessage);
        }

    }
}
