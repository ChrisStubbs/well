namespace PH.Well.UnitTests.Factories
{
    using Well.Domain;
    
    public class UserFactory : EntityFactory<UserFactory, User>
    {
        public UserFactory()
        {
            this.Entity.Id = 1;
            this.Entity.Name = "palmerharvey\\fiona.pond";
        }
    }
}
