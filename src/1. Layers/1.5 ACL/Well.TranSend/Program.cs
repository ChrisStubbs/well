namespace PH.Well.TranSend
{
    using Infrastructure;
    using PH.Well.Common.Contracts;

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
