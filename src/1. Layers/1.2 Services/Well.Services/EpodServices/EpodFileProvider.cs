using System.Linq;

namespace PH.Well.Services.EpodServices
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class EpodFileProvider : IEpodFileProvider
    {
        private readonly IEpodImportService epodImportService;
        private readonly ILogger logger;
        private readonly IRouteHeaderRepository routeHeaderRepository;

        public EpodFileProvider(IEpodImportService epodImportService, ILogger logger, IRouteHeaderRepository routeHeaderRepository)
        {
            this.epodImportService = epodImportService;
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
        }

        public void Import(string filePath, string filename,IImportConfig config)
        {
            var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));

            try
            {
                using (var streamReader = new StreamReader(filePath))
                {
                    var routes = (RouteDelivery)xmlSerializer.Deserialize(streamReader);


                    int branchId = 0;
                    if (!routes.RouteHeaders.First().TryParseBranchIdFromRouteNumber(out branchId) || !config.ProcessDataForBranch((Well.Domain.Enums.Branch)branchId))
                    {
                        logger.LogDebug($"Skip route delivery {filePath}");
                        return;
                    }

                    var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });

                    routes.RouteId = route.Id;

                    this.epodImportService.Import(routes, filename);
                }

                logger.LogDebug($"File {filename} imported!");
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Epod update error in XML file {filename}!", exception);
            }
        }
    }
}