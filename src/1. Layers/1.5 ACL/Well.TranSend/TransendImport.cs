namespace PH.Well.TranSend
{
    using Common.Contracts;
    using Contracts;

    using PH.Well.TranSend.Infrastructure;

    public class TransendImport : ITransendImport
    {
        private readonly IEpodProvider epodProvider;
        private readonly IWebClient webClient;

        public TransendImport(IEpodProvider epodProvider, IWebClient webClient)
        {
            this.epodProvider = epodProvider;
            this.webClient = webClient;
        }

        public void Process()
        {
            epodProvider.Import();
            webClient.DownloadString(Configuration.DashboardRefreshEndpoint);
        }
    }
}
