namespace PH.Well.UnitTests.Factories
{
    using Well.Domain;

    public  class RouteHeaderAttributeFactory:EntityFactory<RouteHeaderAttributeFactory, Attribute>
    {
        public RouteHeaderAttributeFactory()
        {
            this.Entity.Id = 1;
            this.Entity.AttributeId = 1;
            this.Entity.Code = "001";
            this.Entity.Value1 = "value";
        }
    }
}
