namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    public interface IPostImportRepository
    {
        void PostImportUpdate();
        void PostTranSendImport();
        void PostTranSendImportForTobacco();
        void PostTranSendImportShortsTba(IEnumerable<int> jobIds);
    }
}
