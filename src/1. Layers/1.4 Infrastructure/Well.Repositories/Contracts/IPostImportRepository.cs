﻿namespace PH.Well.Repositories.Contracts
{
    public interface IPostImportRepository
    {
        void PostImportUpdate();
        void PostTranSendImport();
        void PostTranSendImportForTobacco();
    }
}
