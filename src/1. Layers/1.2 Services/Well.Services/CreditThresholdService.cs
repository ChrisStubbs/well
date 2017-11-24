namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Repositories.Contracts;

    public class CreditThresholdService : ICreditThresholdService
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;
        private readonly IDbMultiConfiguration connections;

        public CreditThresholdService(ICreditThresholdRepository creditThresholdRepository, IDbMultiConfiguration connections)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.connections = connections;
        }

        public IList<CreditThreshold> GetAll()
        {
            return creditThresholdRepository.GetAll().ToList();
        }

        public void DeleteFromAllDatbases(int id)
        {
            foreach (var connectionString in connections.ConnectionStrings)
            {
                this.creditThresholdRepository.Delete(id, connectionString);
            }
        }

        public void SaveOnAllDatabases(CreditThreshold creditThreshold)
        {
            foreach (var connectionString in connections.ConnectionStrings)
            {
               CreateOrUpdateCreditThreshold(creditThreshold, connectionString);
            }
        }

        private void CreateOrUpdateCreditThreshold(CreditThreshold creditThreshold, string connectionString)
        {
            var existingCreditThreshold = creditThresholdRepository.GetByLevel(creditThreshold.Level, connectionString);
            if (existingCreditThreshold == null)
            {
                creditThresholdRepository.Save(creditThreshold, connectionString);
            }
            else
            {
                creditThreshold.Id = existingCreditThreshold.Id;
                creditThresholdRepository.Update(creditThreshold,connectionString);
            }
        }
    }
}