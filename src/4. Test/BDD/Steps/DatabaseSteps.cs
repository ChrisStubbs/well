namespace PH.Well.BDD.Steps
{
    using Domain.Enums;
    using Framework.Context;

    using Newtonsoft.Json;

    using PH.Well.BDD.Framework;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;

    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;


    [Binding]
    public class DatabaseSteps
    {
        private readonly IContainer container;

        private readonly IWellDapperProxy dapperProxy;

        private readonly IWebClient webClient;

        private readonly ILogger logger;

        public DatabaseSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.dapperProxy = this.container.GetInstance<IWellDapperProxy>();
            this.webClient = this.container.GetInstance<IWebClient>();
            this.logger = this.container.GetInstance<ILogger>();
        }

        [Given("I have a clean database")]
        public void RemoveTestData()
        {
            DeleteAndReseed("JobAttribute");
            DeleteAndReseed("JobDetailAttribute");
            DeleteAndReseed("JobDetailDamage");
            DeleteAndReseed("JobDetail");
            DeleteAndReseed("UserJob");
            DeleteAndReseed("Job");
            DeleteAndReseed("Account");
            DeleteAndReseed("StopAttribute");
            DeleteAndReseed("Stop");
            DeleteAndReseed("RouteHeaderAttribute");
            DeleteAndReseed("RouteHeader");
            DeleteAndReseed("Routes");
            DeleteAndReseed("UserBranch");
            DeleteAndReseed("[User]");
            DeleteAndReseed("Audit");
        }

        private void DeleteAndReseed(string tableName)
        {
            var script = 
                $@"DELETE FROM {tableName}

                -- Use sys.identity_columns to see if there was a last known identity value
                -- for the Table. If there was one, the Table is not new and needs a reset
                IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = '{tableName}' AND last_value IS NOT NULL) 
                    DBCC CHECKIDENT ({tableName}, RESEED, 0);";
            this.dapperProxy.ExecuteSql(script);
        }

        [Given(@"The all deliveries have been marked as clean")]
        public void GivenTheAllDeliveriesHaveBeenMarkedAsClean()
        {

        }

        [Given(@"(.*) deliveries have been marked as clean")]
        public void MarkDeliveriesAsClean(int noOfDeliveries)
        {
            SetDeliveryStatus(PerformanceStatus.Compl, noOfDeliveries);
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
        public void GivenDeliveriesHaveBeenMarkedAsResolved(int noOfDeliveries)
        {
            SetDeliveryStatus(PerformanceStatus.Resolved, noOfDeliveries);
        }

        [Given(@"(.*) deliveries have been marked as exceptions")]
        public void MarkDeliveriesAsException(int noOfDeliveries)
        {
            SetDeliveryStatus(PerformanceStatus.Incom, noOfDeliveries);
        }

        [Given(@"All the deliveries are marked as exceptions")]
        public void GivenAllTheDeliveriesAreMarkedAsExceptions()
        {
            SetDeliveryStatus(PerformanceStatus.Incom, 10000);
        }

        public void SetDeliveryStatus(PerformanceStatus status, int noOfDeliveries)
        {
            this.dapperProxy.ExecuteSql($"UPDATE TOP ({noOfDeliveries}) Job " +
                                     $"SET PerformanceStatusId = {(int)status}, " +
                                     "    JobRef3 =  '9' + JobRef1  ");
        }


        [Given(@"I have selected branch (.*)")]
        public void GivenIHaveSelectedBranch(int branch)
        {
            var user = SetUpUser();
            SetUpUserBranch(user, branch);
        }

        public User SetUpUser()
        {
            this.logger.LogDebug("Calling create user");
            var user = JsonConvert.DeserializeObject<User>(this.webClient.DownloadString(Configuration.WellApiUrl + "create-user-using-current-context"));

            if (user == null) this.logger.LogDebug("User is null");

            this.logger.LogDebug($"User created {user.Name}");

            return user;
        }

        public void SetUpUserBranch(User user, int branch)
        {
            this.logger.LogDebug($"User created {user.Name}");
            this.logger.LogDebug($"Branch created {branch}");
            this.dapperProxy.ExecuteSql($"INSERT INTO UserBranch (UserId, BranchId, CreatedBy, DateCreated, UpdatedBy, DateUpdated) VALUES((SELECT Id FROM [User] WHERE Name = '{user.Name}'), {branch}, 'BDD', GETDATE(), 'BDD', GETDATE()); ");
        }
    }
}


