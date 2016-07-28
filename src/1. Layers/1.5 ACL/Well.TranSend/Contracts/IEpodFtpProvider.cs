namespace PH.Well.TranSend.Contracts
{
    using System.Collections.Generic;

    public interface IEpodFtpProvider
    {
        void ListFilesAndProcess(List<string> schemaErrors);
    }
}