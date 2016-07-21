namespace PH.Well.BDD.Steps
{
    using Domain.Enums;
    using Framework.Context;
    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;


    [Binding]
    public class DatabaseSteps
    {
        private readonly IContainer container;

        private readonly IWellDapperProxy dapperProxy;

        public DatabaseSteps()
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

        [Given(@"The all deliveries have been marked as clean")]
        public void GivenTheAllDeliveriesHaveBeenMarkedAsClean()
        {

        }

        [Given(@"(.*) deliveries have been marked as clean")]
        public void MarkDeliveriesAsClean(int noOfCleanDeliveries)
        {
            SetDeliveryStatus(PerformanceStatus.Compl, noOfCleanDeliveries);
        }

        [Given(@"All the deliveries are marked as clean")]
        public void GivenAllTheDeliveriesAreMarkedAsClean()
        {
            SetDeliveryStatus(PerformanceStatus.Compl, 10000);
        }

        [Given(@"All the deliveries are marked as Resolved")]
        public void GivenAllTheDeliveriesAreMarkedAsResolved()
        {
            SetDeliveryStatus(PerformanceStatus.Resolved, 10000);
        }

        [Given(@"(.*) deliveries have been marked as Resolved")]
        public void GivenDeliveriesHaveBeenMarkedAsResolved(int noOfCleanDeliveries)
        {
            SetDeliveryStatus(PerformanceStatus.Resolved, noOfCleanDeliveries);
        }

        public void SetDeliveryStatus(PerformanceStatus status, int noOfCleanDeliveries)
        {
            this.dapperProxy.ExecuteSql($"UPDATE TOP ({noOfCleanDeliveries}) Job " +
                                     $"SET PerformanceStatusId = {(int)status}, " +
                                     "    JobRef3 =  '9' + JobRef1  ");
        }

    }

}
