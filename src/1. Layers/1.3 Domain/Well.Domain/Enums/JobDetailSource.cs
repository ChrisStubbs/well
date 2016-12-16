namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

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
}
