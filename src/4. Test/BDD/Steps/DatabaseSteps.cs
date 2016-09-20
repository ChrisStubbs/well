namespace PH.Well.BDD.Steps
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
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
        private IAuditRepository auditRepo;

        public DatabaseSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.dapperProxy = this.container.GetInstance<IWellDapperProxy>();
            this.webClient = this.container.GetInstance<IWebClient>();
            this.logger = this.container.GetInstance<ILogger>();
            auditRepo = container.GetInstance<IAuditRepository>();
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

        [Given(@"25 audit entries have been made")]
        public void InsertAudits()
        {
            auditRepo.CurrentUser = "BDD.User";
            for (int i = 0; i < 25; i++)
            {
                var audit = new Audit()
                {
                    Entry = "Audit 123",
                    AccountCode = "123456",
                    InvoiceNumber = "987654",
                    DeliveryDate = new DateTime(2016, 1, 20)
                };
                auditRepo.Save(audit);
            }
        }

        [Given(@"5 audit entries have been made")]
        public void Insert5Audits()
        {
            auditRepo.CurrentUser = "BDD.User";
           
                var audit = new Audit()
                {
                    Entry = "Audit 123",
                    Type = AuditType.DeliveryLineUpdate,
                    AccountCode = "123456",
                    InvoiceNumber = "987654",
                    DeliveryDate = new DateTime(2016, 1, 20)
                };
            auditRepo.Save(audit);
            auditRepo.Save(audit);

            var audit2 = new Audit()
            {
                Entry = "Audit 456",
                Type = AuditType.DeliveryLineUpdate,
                AccountCode = "88888",
                InvoiceNumber = "55555",
                DeliveryDate = new DateTime(2016, 5, 15)
            };
            auditRepo.Save(audit2);
            auditRepo.Save(audit2);
            auditRepo.Save(audit2);
        }

        [When(@"valid invoice numbers are assigned to jobs")]
        public void WhenValidInvoiceNumbersAreAssignedToJobs()
        {
            AssignInviceNumbers(JobDetailStatus.Res);
        }


        public void SetDeliveryStatus(PerformanceStatus status, int noOfDeliveries)
        {
            this.dapperProxy.ExecuteSql($"UPDATE TOP ({noOfDeliveries}) Job " +
                                     $"SET PerformanceStatusId = {(int)status}, " +
                                     "    JobRef3 =  '9' + JobRef1  ");
        }

        public void AssignInviceNumbers(JobDetailStatus jobDetailStatus)
        {
            this.dapperProxy.ExecuteSql($"UPDATE JobDetail " +
                                     $"SET JobDetailStatusId = {(int)jobDetailStatus}");
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


