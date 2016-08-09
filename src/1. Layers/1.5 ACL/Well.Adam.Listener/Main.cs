namespace Well.Adam.Listener
{
    using System;
    using System.Windows.Forms;

    using PH.Well.Adam.Contracts;
    using PH.Well.Adam.Infrastructure;
    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Repositories;

    using StructureMap;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;
    using PH.Well.Services.EpodImport;

    public partial class Main : Form
    {
        private IContainer container;

        public Main()
        {
            InitializeComponent();

            this.container = this.InitIoc();

            var monitorService = this.container.GetInstance<IMonitorService>();

            monitorService.Monitor(PH.Well.Adam.Listener.Configuration.RootFolder);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<IEpodSchemaProvider>().Use<EpodSchemaProvider>();
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IEpodDomainImportProvider>().Use<EpodDomainImportProvider>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IEpodDomainImportService>().Use<EpodDomainImportService>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IAdamRouteFileProvider>().Use<AdamRouteFileProvider>();
                    x.For<IAdamImportConfiguration>().Use<PH.Well.Adam.Infrastructure.Configuration>();
                    x.For<IMonitorService>().Use<MonitorService>();
                    x.For<IFileService>().Use<FileService>();
                    x.For<IFileModule>().Use<FileModule>();
                });
        }



    }
}
