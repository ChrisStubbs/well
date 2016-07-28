namespace PH.Well.UnitTests.Factories
{

    using Attribute = Well.Domain.Attribute;
    public  class StopAttributeFactory : EntityFactory<StopAttributeFactory, Attribute>
    {
        public StopAttributeFactory()
        {
            this.Entity.Id = 1;
            this.Entity.AttributeId = 1;
            this.Entity.Code = "001";
            this.Entity.Value1 = "value";
        
        }
    }
}
