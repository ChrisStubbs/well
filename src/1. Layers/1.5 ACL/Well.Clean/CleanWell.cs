namespace PH.Well.Clean
{
    using PH.Well.Services.Contracts;

    using StructureMap;

    public class CleanWell : ICleanWell
    {
        private readonly ICleanDeliveryService cleanDeliveryService;

        public CleanWell(ICleanDeliveryService cleanDeliveryService)
        {
            this.cleanDeliveryService = cleanDeliveryService;
        }
        public void Process(IContainer container)
        {

            cleanDeliveryService.DeleteCleans();
        }
    }
}
  