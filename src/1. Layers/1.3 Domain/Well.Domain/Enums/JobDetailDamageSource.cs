namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum JobDetailDamageSource
    {
        [Description("The Warehouse")]
        PSTDIS001 = 1,

        [Description("Manufacturer")]
        PDADIS001 = 2,

        [Description("Not Defined")]
        PDRDIS002 = 3,

        [Description("Not Defined")]
        NotDef = 0,
    }
}
