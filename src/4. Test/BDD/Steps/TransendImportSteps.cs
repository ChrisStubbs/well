namespace PH.Well.BDD.Steps
{
    using Framework.Context;
    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;


    [Binding]
    public class TransendImportSteps
    {

        private readonly IContainer container;
        private readonly IWellDapperProxy dapperProxy;
        public TransendImportSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.dapperProxy = this.container.GetInstance<IWellDapperProxy>();
        }

      


    }
}
