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
    using PH.Well.Services.EpodServices;

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            var container = InitIoc();

            var logger = container.GetInstance<ILogger>();

            logger.LogDebug("Starting ADAM listener!");

            try
            {
                var monitorService = container.GetInstance<IAdamFileMonitorService>();

                monitorService.Monitor(Configuration.RootFolder);
            }
            catch (Exception exception)
            {
                logger.LogError("Error!", exception);
            }
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
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IAccountRepository>().Use<AccountRepository>();
                    x.For<IAdamFileMonitorService>().Use<AdamFileMonitorService>();
                    x.For<IRouteMapper>().Use<RouteMapper>();
                    x.For<IFileService>().Use<FileService>();
                    x.For<IFileModule>().Use<FileModule>();
                    x.For<IFileTypeService>().Use<FileTypeService>();
                    x.For<IAdamImportService>().Use<AdamImportService>();
                    x.For<IAdamUpdateService>().Use<AdamUpdateService>();
                });
        }
    }
}
