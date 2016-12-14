namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum JobDetailReason
    {
        [Description("Not Defined")]
        NotDefined = 0,
        [Description("No Credit")]
        NoCredit = 1,
        [Description("Damaged Goods")]
        DamagedGoods = 2,
        [Description("Short Delivered")]
        ShortDelivered = 3,
        [Description("Booking Error")]
        BookingError = 4,
        [Description("Picking Error")]
        PickingError = 5,
        [Description("Other Error")]
        OtherError = 6,
        [Description("Administration")]
        Administration = 7,
        [Description("Accumulated Damages")]
        AccumulatedDamages = 8,
        [Description("Recall Product")]
        RecallProduct = 9,
        [Description("Customer Damaged")]
        CustomerDamaged = 10,
        [Description("Short Dated")]
        ShortDated = 11,
        [Description("Vouchers")]
        Vouchers = 12,
        [Description("Signed Short")]
        SignedShort = 13,
        [Description("Out Of Date Stock")]
        OutOfDateStock = 14,
        [Description("Short T.B.A.")]
        ShortTBA = 15,
        [Description("Availability Guarantee")]
        AvailabilityGuarantee = 16,
        [Description("Freezer Chiller Breakdown")]
        FreezerChillerBreakdown = 17,
        [Description("Not Enough Room")]
        NotEnoughRoom = 18,
        [Description("Out Of Temp")]
        OutOfTemp = 19,
        [Description("Duplicate Order")]
        DuplicateOrder = 20,
        [Description("Not Ordered")]
        NotOrdered = 21,
        [Description("Shop Closed No Staff")]
        ShopClosedNoStaff = 22,
        [Description("Minimum Drop Charge")]
        MinimumDropCharge = 23
    }
}