namespace PH.Well.Adam
{
    using Contracts;
    using StructureMap;

    public class Import
    {
        public void Process(IContainer container)
        {
            var adamRouteFileProvider = container.GetInstance<IAdamRouteFileProvider>();
            adamRouteFileProvider.ListFilesAndProcess();
        }
    }
}
