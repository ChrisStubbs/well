namespace Well.Clean
{
    using Infrastructure;
    using PH.Well.Common.Contracts;

    class Program
    {
        static void Main(string[] args)
        {

            string epodStatusMessage = string.Empty;
            var container = DependancyRegister.InitIoc();
            var logger = container.GetInstance<ILogger>();
            new CleanWell().Process(container, ref epodStatusMessage);
            logger.LogDebug(epodStatusMessage);
        }
    }
}
