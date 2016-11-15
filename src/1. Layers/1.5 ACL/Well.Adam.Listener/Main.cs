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
                    x.For<IEpodSchemaValidator>().Use<EpodSchemaValidator>();
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IEpodImportProvider>().Use<EpodImportProvider>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IEpodImportService>().Use<EpodImportService>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IAccountRepository>().Use<AccountRepository>();
                    x.For<IAdamFileMonitorService>().Use<AdamFileMonitorService>();
                    x.For<IFileService>().Use<FileService>();
                    x.For<IFileModule>().Use<FileModule>();
                    x.For<IFileTypeService>().Use<FileTypeService>();
                });
        }
    }
}
