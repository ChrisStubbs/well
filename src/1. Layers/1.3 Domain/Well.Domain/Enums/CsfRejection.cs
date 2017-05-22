namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum CsfRejection
    {
        [Description("Unreturned Goods")]
        UnreturnedGoods = 1,

        [Description("Damaged Returns")]
        DamagedReturns = 2,

        [Description("UnPaid")]
        UnPaid = 3,

        [Description("Not Signed Short")]
        NotSignedShort = 4,

        [Description("Products Found")]
        ProductsFound = 5,

        [Description("Duplicate Claim")]
        DuplicateClaim = 6,
    }
}
