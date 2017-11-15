//namespace PH.Well.Repositories
//{
//    using Api.Infrastructure;
//    using Contracts;
//    using PH.Common.Security.Interfaces;
//    using Shared.Well.Data.EF;

//    public class WellEntitiesFactory : IWellEntitiesFactory
//    {
//        private readonly IUserNameProvider userNameProvider;
//        private readonly IWellDbConfiguration wellDbConfiguration;

//        public WellEntitiesFactory(IUserNameProvider userNameProvider, IWellDbConfiguration wellDbConfiguration)
//        {
//            this.userNameProvider = userNameProvider;
//            this.wellDbConfiguration = wellDbConfiguration;
//        }

//        public WellEntities GetWellEntities()
//        {
//            var connectionStringOrName = $"connectionstring={wellDbConfiguration.DatabaseConnection}";
//            return new WellEntities(userNameProvider); ;
//        }
        
//    }
//}