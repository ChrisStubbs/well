namespace PH.Well.BDD.Steps
{
    using System;
    using System.Linq;

    using Domain.Enums;
    using Framework.Context;

    using Newtonsoft.Json;

    using NUnit.Framework;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using Repositories;
    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;
    using Branch = Domain.Branch;
    using Configuration = Framework.Configuration;

    [Binding]
    public class DatabaseSteps
    {
        private readonly IContainer container;

        private readonly IWellDapperProxy dapperProxy;

        private readonly IWebClient webClient;

        private readonly ILogger logger;

        private IAuditRepository auditRepo;

        private INotificationRepository notificationRepository;

        private ICreditThresholdRepository creditThresholdRepository;

        public DatabaseSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(
                ContextDescriptors.StructureMapContainer);
            this.dapperProxy = this.container.GetInstance<IWellDapperProxy>();
            this.webClient = this.container.GetInstance<IWebClient>();
            this.logger = this.container.GetInstance<ILogger>();
            this.auditRepo = this.container.GetInstance<IAuditRepository>();
            this.notificationRepository = this.container.GetInstance<INotificationRepository>();
            this.creditThresholdRepository = this.container.GetInstance<ICreditThresholdRepository>();
        }

        [Given("I have a clean database")]
        public void RemoveTestData()
        {
            DeleteAndReseed("PendingCredit");
            DeleteAndReseed("JobDetailDamage");
            DeleteAndReseed("JobDetailAction");
            DeleteAndReseed("JobDetail");
            DeleteAndReseed("UserJob");
            DeleteAndReseed("Notification");
            DeleteAndReseed("Job");
            DeleteAndReseed("Account");
            DeleteAndReseed("Stop");
            DeleteAndReseed("RouteHeader");
            DeleteAndReseed("Routes");
            DeleteAndReseed("UserBranch");
            DeleteAndReseed("[User]");
            DeleteAndReseed("Audit");
            DeleteAndReseed("SeasonalDateToBranch");
            DeleteAndReseed("SeasonalDate");
            DeleteAndReseed("CleanPreferenceToBranch");
            DeleteAndReseed("CleanPreference");
            DeleteAndReseed("CreditThresholdToBranch");
            DeleteAndReseed("CreditThreshold");
            DeleteAndReseed("WidgetToBranch");
            DeleteAndReseed("Widget");
        }

        private void DeleteAndReseed(string tableName)
        {
            //Use sys.identity_columns to see if there was a last known identity value
            //for the Table. If there was one, the Table is not new and needs a reset
            var script = $@"DELETE FROM {tableName}; ";
            script += $@"IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = '{tableName}' AND last_value IS NOT NULL) 
                            DBCC CHECKIDENT ({tableName}, RESEED, 0);";
            this.dapperProxy.ExecuteSql(script);
        }

        [Given(@"(.*) deliveries have been marked as clean")]
        public void MarkDeliveriesAsClean(int noOfDeliveries)
        {
            this.SetDeliveryStatusToClean(noOfDeliveries);

            this.dapperProxy.ExecuteSql("update jobdetail set ShortsStatus = 1");
        }

        [Given(@"All the deliveries are marked as clean")]
        public void GivenAllTheDeliveriesAreMarkedAsClean()
        {
            this.SetDeliveryStatusToClean(10000);
            this.dapperProxy.ExecuteSql("update jobdetail set ShortsStatus = 1");
        }

        [Given(@"The clean deliveries are '(.*)' days old")]
        public void CleanDeliveriesAreThisOld(int daysOld)
        {
            var cleanDate = DateTime.Now.AddDays(daysOld);

            this.dapperProxy.ExecuteSql($"UPDATE JobDetail SET DateUpdated = '{cleanDate}'");
        }

        [Given(@"Clean deliveries are updated to (.*) days old")]
        public void CleanDeliveriesAreUpdated(int daysOld)
        {
            var cleanDate = DateTime.Now.AddDays(daysOld);

            this.dapperProxy.ExecuteSql($"UPDATE Stop SET DeliveryDate = '{cleanDate}'");
        }


        [Given(@"All the deliveries are marked as Resolved")]
        public void GivenAllTheDeliveriesAreMarkedAsResolved()
        {
            this.SetDeliveryStatusToResolved(10000);
        }

        [Given(@"(.*) deliveries have been marked as Resolved")]
        public void GivenDeliveriesHaveBeenMarkedAsResolved(int noOfDeliveries)
        {
            this.SetDeliveryStatusToResolved(noOfDeliveries);
        }

        [Given(@"(.*) deliveries have been marked as exceptions")]
        public void MarkDeliveriesAsException(int noOfDeliveries)
        {
            this.SetDeliveryStatusToException(noOfDeliveries, false);
        }

        [Given(@"(.*) deliveries have been marked as exceptions with shorts to be advised")]
        public void MarkDeliveriesAsExceptionWithShortsToBeAdvised(int noOfDeliveries)
        {
            this.SetDeliveryStatusToException(noOfDeliveries, true);
        }

        [Given(@"All the deliveries are marked as exceptions")]
        public void GivenAllTheDeliveriesAreMarkedAsExceptions()
        {
            this.SetDeliveryStatusToException(10000, false);
        }

        [Given(@"All delivery lines are flagged with line delivery status 'Exception'")]
        public void GivenAllTheDeliveryLinesAreFlaggedAsLineDeliveryStatusException()
        {
            this.MakeJobDetailsLineDeliveryStatusException();
        }

        [Given(@"25 audit entries have been made")]
        public void InsertAudits()
        {
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
            this.AssignInvoiceNumbers(JobDetailStatus.Res);
        }

        public void MakeJobDetailsLineDeliveryStatusException()
        {
            this.dapperProxy.ExecuteSql("UPDATE JobDetail Set LineDeliveryStatus = 'Exception'");
        }

        [Given(@"there are no proof of deliveries")]
        public void RemoveAllProofOfDeliveries()
        {
            this.dapperProxy.ExecuteSql("update job set proofofdelivery = null");
        }

        public void SetDeliveryStatusToClean(int noOfDeliveries)
        {
            this.dapperProxy.ExecuteSql($"UPDATE TOP ({noOfDeliveries}) Job SET JobStatusId = 3, InvoiceNumber =  '9' + PickListRef");
        }

        public void SetDeliveryStatusToException(int noOfDeliveries, bool setJobToBeAdvised)
        {
            var jobIds = string.Join(",", this.dapperProxy.SqlQuery<int>($"Select TOP ({noOfDeliveries}) Id from Job"));

            var shortQty = setJobToBeAdvised ? "2" : "1";

            var jobSql = "UPDATE Job SET JobStatusId = 4" +
                         (setJobToBeAdvised ? $", OuterCount = 10, OuterDiscrepancyFound = 1, TotalOutersShort = {shortQty} " : "") +
                         $"Where Id in ({jobIds})";

            this.dapperProxy.ExecuteSql(jobSql);

            this.dapperProxy.ExecuteSql($"UPDATE JobDetail SET ShortQty = {shortQty}, ShortsStatus = 2 Where JobId in ({jobIds})");
        }

        public void SetDeliveryStatusToResolved(int noOfDeliveries)
        {
            this.dapperProxy.ExecuteSql($"UPDATE TOP ({noOfDeliveries}) Job SET JobStatusId = 5, InvoiceNumber =  '9' + PickListRef");
        }

        public void AssignInvoiceNumbers(JobDetailStatus jobDetailStatus)
        {
            this.dapperProxy.ExecuteSql($"UPDATE JobDetail " +
                                     $"SET ShortsStatus = {(int)jobDetailStatus}");
        }

        [Given(@"I have selected branch '(.*)'")]
        public void GivenIHaveSelectedBranch(int branch)
        {
            var user = SetUpUser();
            SetUpUserBranch(user.Name, branch);
        }

        [Given(@"I have selected branches '(.*)' and '(.*)'")]
        public void GivenIHaveSelectedBranchesAnd(int branch1, int branch2)
        {
            var user = SetUpUser();
            SetUpUserBranch(user.Name, branch1);
            SetUpUserBranch(user.Name, branch2);
        }

        [Given(@"I have selected branch (.*) for user identity: (.*)")]
        public void GivenIHaveSelectedBranch(int branch, string userIdentity)
        {
            var user = SetUpUser(userIdentity);
            SetUpUserBranch(user.Name, branch);
        }

        public User SetUpUser()
        {
            return SetUpUser($"{Environment.UserDomainName}\\{Environment.UserName}");
        }

        public User SetUpUser(string userIdentity)
        {
            this.logger.LogDebug("Calling create user");
            var user = JsonConvert.DeserializeObject<User>(this.webClient.UploadString(Configuration.WellApiUrl + $"create-user?userIdentity={userIdentity}", "POST", ""));

            if (user == null)
            {
                this.logger.LogDebug("User is null");
            }
            else
            {
                this.logger.LogDebug($"User created {user.Name}");
            }

            return user;
        }

        public void SetUpUserBranch(string userName, int branch)
        {
            this.logger.LogDebug($"User created {userName}");
            this.logger.LogDebug($"Branch created {branch}");
            this.dapperProxy.ExecuteSql($"INSERT INTO UserBranch (UserId, BranchId, CreatedBy, DateCreated, UpdatedBy, DateUpdated) VALUES((SELECT Id FROM [User] WHERE Name = '{userName}'), {branch}, 'BDD', GETDATE(), 'BDD', GETDATE()); ");
        }

        [Then(@"the clean deliveries are removed from the well")]
        public void CleanDeliveriesSoftDeleted()
        {
            var result = this.dapperProxy.SqlQuery<int>("select count(1) from JobDetail where isDeleted = 0").Single();

            Assert.That(result, Is.EqualTo(0));
        }
        
        [Then(@"the first (.*) rows are credited and no longer on the exceptions grid")]
        public void ThenTheFirstRowsAreCreditedAndNoLongerOnTheExceptionsGrid(int rows)
        {
            var result = this.dapperProxy.SqlQuery<int>("select count(1) from job where JobStatusId = 5").Single();

            Assert.That(result, Is.EqualTo(rows));
        }

        [Given(@"(.*) deliveries have been assigned starting with job (.*)")]
        public void AssignDeliveries(int deliveries, int jobId)
        {
            for (int i = 0; i < deliveries; i++)
            {
                this.dapperProxy.ExecuteSql($"INSERT INTO UserJob (UserId, JobId, CreatedBy, DateCreated, UpdatedBy, DateUpdated) VALUES((SELECT TOP 1 Id FROM [User]), {jobId + i}, 'BDD', GETDATE(), 'BDD', GETDATE()); ");
            }
        }
        
        [Given(@"(.*) notifications have been made")]
        public void InsertNotifications(int notifications)
        {
            for (int i = 0; i < notifications; i++)
            {
                var accountNumber = 12345.001;
                var invoiceNumber = 440000;

                var notification = new Notification
                {
                    JobId = i,
                    ErrorMessage = "Credit failed ADAM validation",
                    Type = (int)NotificationType.Credit,
                    Account = (accountNumber + i).ToString(),
                    InvoiceNumber = (invoiceNumber + i).ToString(),
                    Branch = "55",
                    UserName = "FP",
                    AdamErrorNumber = "1",
                    AdamCrossReference = "123",
                    Source = "BDD"
                };

                notificationRepository.SaveNotification(notification);

            }
        }

        [Given(@"'(.*)' clean deliveries are updated to branch '(.*)'")]
        public void GivenCleanDeliveriesAreUpdateTo(int noOfDeliveries, int location)
        {
            this.dapperProxy.ExecuteSql($"UPDATE TOP({noOfDeliveries}) RouteHeader SET StartDepotCode = '{location}', RouteOwnerId = '{location}'");
        }


        [Then(@"a threshold level of (.*) has a value of (.*)")]
        public void ThenAThresholdLevelOfHasAValueOf(int thresholdLevel, decimal thresholdValue)
        {
            var sql = $"SELECT Threshold FROM [dbo].[CreditThreshold] WHERE ThresholdLevelId = {thresholdLevel}";
            var value = this.dapperProxy.SqlQuery<decimal>(sql).FirstOrDefault();
            Assert.That(value, Is.EqualTo(thresholdValue));
        }

        [Then(@"I have a threshold level of (.*)")]
        public void ThenIHaveAThresholdLevelOf(int thresholdLevel)
        {
            this.dapperProxy.ExecuteSql($"UPDATE [dbo].[User] SET ThresholdLevelId= {thresholdLevel}");
        }
        
        [Then(@"I have (.*) resolved deliveries")]
        public void ThenIHaveResolvedDeliveries(int resolvedDeliveries)
        {
            var sql = $"SELECT COUNT(Id) FROM [dbo].[Job] WHERE JobStatusId = 5";
            var value = this.dapperProxy.SqlQuery<int>(sql).FirstOrDefault();
            Assert.That(value, Is.EqualTo(resolvedDeliveries));
        }

        [Then(@"I have (.*) pending and (.*) resolved delivery")]
        public void ThenIHavePendingAndResolvedDelivery(int pendingDelivery, int resolvedDelivery)
        {
            var sql = $"SELECT COUNT(Id) FROM [dbo].[Job] WHERE JobStatusId = 2";
            var pendingValue = this.dapperProxy.SqlQuery<int>(sql).FirstOrDefault();

            sql = $"SELECT COUNT(Id) FROM [dbo].[Job] WHERE JobStatusId = 5";
            var resolvedValue = this.dapperProxy.SqlQuery<int>(sql).FirstOrDefault();

            Assert.That(pendingValue, Is.EqualTo(pendingDelivery));
            Assert.That(resolvedValue, Is.EqualTo(resolvedDelivery));
        }

        [Given(@"I have the following credit threshold levels set in the database")]
        public void GivenIHaveTheFollowingCreditThresholdLevelsSetInTheDatabase(Table table)
        {
            foreach (var row in table.Rows)
            {
                var ct = new CreditThreshold
                {
                    ThresholdLevelId =  int.Parse(row["Level"]),
                    Threshold = decimal.Parse(row["Value"]),
                };

                ct.Branches.Add(new Branch {Id = int.Parse(row["Branch"])});
                creditThresholdRepository.Save(ct);
            }
        }

    }
}


