namespace PH.Well.TranSend
{
    using System.IO;
    using System.Reflection;
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
            string epodStatusMessage = string.Empty;
            var container = DependancyRegister.InitIoc();
            var logger = container.GetInstance<ILogger>();
            new Import().Process(container, ref  epodStatusMessage);      
            logger.LogDebug(epodStatusMessage);
        }
    }
}
