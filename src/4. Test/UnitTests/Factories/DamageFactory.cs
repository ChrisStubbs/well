namespace PH.Well.UnitTests.Factories
{
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    
    public class DamageFactory : EntityFactory<DamageFactory, Damage>
    {
        public DamageFactory()
        {
            this.Entity.JobDetailId = 1;
            this.Entity.Quantity = 5;
            this.Entity.Reason = DamageReasons.CAR01;
        }
    }
}
