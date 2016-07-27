namespace PH.Well.Adam.Contracts
{
    using System.Collections.Generic;

    public  interface IAdamRouteFileProvider
    {
        void ListFilesAndProcess(IAdamImportConfiguration config, out List<string> schemaErrors);
    }
}
