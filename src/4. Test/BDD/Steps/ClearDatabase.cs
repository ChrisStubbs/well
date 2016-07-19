namespace PH.Well.BDD.Steps
{
    using Framework.Context;
    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;


    [Binding]
    public class ClearDatabase
    {
        private readonly IContainer container;

        private readonly IWellDapperProxy dapperProxy;

        public ClearDatabase()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.dapperProxy = this.container.GetInstance<IWellDapperProxy>();
        }

        [Given("I have a clean database")]
        public void RemoveTestData()
        {
            this.dapperProxy.ExecuteSql("DELETE FROM JobAttribute");
            this.dapperProxy.ExecuteSql("DELETE FROM JobDetailAttribute");
            this.dapperProxy.ExecuteSql("DELETE FROM JobDetail");
            this.dapperProxy.ExecuteSql("DELETE FROM Job");
            this.dapperProxy.ExecuteSql("DELETE FROM Account");
            this.dapperProxy.ExecuteSql("DELETE FROM StopAttribute");
            this.dapperProxy.ExecuteSql("DELETE FROM Stop");
            this.dapperProxy.ExecuteSql("DELETE FROM RouteHeaderAttribute");
            this.dapperProxy.ExecuteSql("DELETE FROM RouteHeader");
            this.dapperProxy.ExecuteSql("DELETE FROM Routes");
        }
    }

}
