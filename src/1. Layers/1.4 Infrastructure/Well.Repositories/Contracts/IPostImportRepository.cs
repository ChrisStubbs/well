namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    public interface IPostImportRepository
    {
        void PostImportUpdate();
        void PostTranSendImport(IEnumerable<int> jobIds);
        void PostTranSendImportForTobacco(IEnumerable<int> jobIds);
        void PostTranSendImportShortsTba(IEnumerable<int> jobIds);
    }
}
