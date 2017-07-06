namespace PH.Well.Clean
{
    using PH.Well.Services.Contracts;

    using StructureMap;

    public class CleanWell
    {
        public void Process(IContainer container)
        {
            var cleanService = container.GetInstance<ICleanDeliveryService>();

            cleanService.DeleteCleans();
        }
    }
}
  