namespace PH.Well.Adam.Listener
{
    using System;
    using System.Windows.Forms;

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
        public Main()
        {
            InitializeComponent();

            var container = InitIoc();

            var monitorService = container.GetInstance<IAdamFileMonitorService>();

            monitorService.Monitor(Configuration.RootFolder);
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Visible = false;
            this.ShowInTaskbar = false;

            base.OnLoad(e);
        }

        private static Container InitIoc()
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
                    x.For<IAdamFileMonitorService>().Use<AdamFileMonitorService>();
                    x.For<IFileService>().Use<FileService>();
                    x.For<IFileModule>().Use<FileModule>();
                });
        }
    }
}
