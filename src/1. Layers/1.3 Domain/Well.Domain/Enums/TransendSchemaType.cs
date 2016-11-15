namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum TransendSchemaType
    {
        [Description("TransendPHRoutesSchema.xsd")]
        RouteHeaderSchema = 1,

        [Description("TransendPHEpodSchema.xsd")]
        RouteEpodSchema = 2,

        [Description("TransEndPHOrderUpdateSchema.xsd")]
        RouteUpdateSchema = 3,

        Unknown = 4
    }
}
