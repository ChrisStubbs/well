namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ICreditThresholdService
    {
        void DeleteFromAllDatbases(int id);
        IList<CreditThreshold> GetAll();
        void SaveOnAllDatabases(CreditThreshold creditThreshold);
    }
}