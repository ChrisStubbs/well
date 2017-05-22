namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public  enum AccountType
    {
        [Description("Customer")]
        Cust = 1,

        [Description("Other")]
        Other = 2,

        [Description("Outbase")]
        Out = 3,

        [Description("Store")]
        Store = 4,

        [Description("Supplier")]
        Supplier = 5,
    }
}
