namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;
    using PH.Well.Domain.Extensions;

    public enum JobDetailSource
    {
        [Description("Not Defined")]
        NotDefined = 0,
        Input = 1,
        Assembler = 2,
        Checker = 3,
        Packer = 4,
        Confirming = 5,
        Delivery = 6,
        [Description("Rep Telesales")]
        RepTelesales = 7,
        [Description("Product Fault")]
        ProductFault = 8,
        Customer = 9
    }

    public static class JobDetailSourceDescriptions
    {
        private static readonly string repTelesales;
        private static readonly string productFault;

        static JobDetailSourceDescriptions()
        {
            repTelesales = EnumExtensions.GetDescription(JobDetailSource.RepTelesales);
            productFault = EnumExtensions.GetDescription(JobDetailSource.ProductFault);
        }

        public static string Description(this JobDetailSource value)
        {
            switch (value)
            {
                case JobDetailSource.Input:
                    return "Input";

                case JobDetailSource.Assembler:
                    return "Assembler";

                case JobDetailSource.Checker:
                    return "Checker";

                case JobDetailSource.Packer:
                    return "Packer";

                case JobDetailSource.Confirming:
                    return "Confirming";

                case JobDetailSource.Delivery:
                    return "Driver";

                case JobDetailSource.RepTelesales:
                    return repTelesales;

                case JobDetailSource.ProductFault:
                    return productFault;

                case JobDetailSource.Customer:
                    return "Customer";

                default:
                    return null;
            }
        }
    }
}
