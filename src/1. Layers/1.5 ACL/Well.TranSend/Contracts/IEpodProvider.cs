﻿namespace PH.Well.TranSend.Contracts
{
    using System.Collections.Generic;

    public interface IEpodProvider
    {
        void ListFilesAndProcess(List<string> schemaErrors);
    }
}