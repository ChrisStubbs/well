namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum ByPassReasons
    {
        [Description("No Adult Signature")]
        Minus16 = 1,

        [Description("Instructions Required")]
        Cib = 2,

        [Description("Product Faulty")]
        F = 3,

        [Description("Incorrect Details")]
        Id = 4,

        [Description("Insecure Premises")]
        Ip = 5,

        [Description("Loan Required")]
        Lr = 6,

        [Description("Customer not at Home")]
        Nbi = 7,

        [Description("Job Part Complete")]
        Pc = 8,

        [Description("Customer Cancelled on Pre-call")]
        Z = 9,

        [Description("Store Cancelled - file not complete")]
        Z1 = 10,

        [Description("Store Cancelled - Stock not ready")]
        Z2 = 11,

        [Description("No spur plate on pre-call")]
        Z3 = 12,

        [Description("Not Defined")]
        Notdef = 13,
    }
}
