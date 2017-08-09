namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IRouteFileImportCommands : IImportCommands
    {
        void DeleteStopsNotInFile(IEnumerable<Stop> existingRouteStopsFromDb, List<StopDTO> stops);
    }
}