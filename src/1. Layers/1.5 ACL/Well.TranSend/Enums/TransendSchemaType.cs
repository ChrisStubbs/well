namespace PH.Well.TranSend.Enums
{
    using System.ComponentModel;

    public enum TransendSchemaType
    {
        [Description("TransendPHRoutesSchema.xsd")]
        RouteHeaderSchema = 1,

        [Description("ePOD_")]
        RouteEpodSchema = 2,

    }
}
