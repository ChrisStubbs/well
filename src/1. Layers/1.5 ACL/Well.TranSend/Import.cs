namespace PH.Well.TranSend
{
    using Common.Contracts;
    using Contracts;

    using PH.Well.TranSend.Infrastructure;

    using StructureMap;

    public class Import
    {
        public void Process(IContainer container)
        {
            var ePodProvider = container.GetInstance<IEpodProvider>();
            var webClient = container.GetInstance<IWebClient>();

            ePodProvider.Import();

            webClient.DownloadString(Configuration.DashboardRefreshEndpoint);
        }
    }
}
