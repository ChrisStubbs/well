using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.BDD.Steps
{
    using Adam;
    using Adam.Contracts;
    using Framework.Context;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class AdamImportSteps
    {
        private readonly IContainer container;

        public AdamImportSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
        }

        [Given(@"I have loaded the Adam route data")]
        public void LoadAdamRouteData()
        {
            var adamImport = new Import();
            
            adamImport.Process(container);
        }
    }
}
