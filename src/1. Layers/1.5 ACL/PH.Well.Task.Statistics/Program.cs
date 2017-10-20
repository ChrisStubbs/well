using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Shared.Well.Data.EF;
using PH.Well.Common;
using PH.Well.Common.Contracts;
using PH.Well.Domain.Enums;
using PH.Well.Repositories;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Services.Contracts;
using StructureMap;

namespace PH.Well.Task.Statistics
{
    public class Program
    {
        static void Main(string[] args)
        {
            var container = InitIoc();

            var routeStatistics = container.GetInstance<RouteStatistics>();
            routeStatistics.UpdateRouteStatistics();
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<IRouteService>().Use<RouteService>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IWellStatusAggregator>().Use<WellStatusAggregator>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<INotificationRepository>().Use<NotificationRepository>();
                    x.For<IUserNameProvider>().Use<UserNameProvider>();
                    x.For<PH.Common.Security.Interfaces.IUserNameProvider>().Use<UserNameProvider>();
                    x.For<WellEntities>().Use<WellEntities>();
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IDapperProxy>().Use<WellDapperProxy>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<RouteStatistics>().Use<RouteStatistics>();
                });
        }
    }

    public  class UserNameProvider : IUserNameProvider, PH.Common.Security.Interfaces.IUserNameProvider
    {
        public string GetUserName()
        {
            return "Well Statistics";
        }
    }
}
