namespace PH.Well.BDD.Steps.Page
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Common.Contracts;
    using Domain;
    using Framework.Context;
    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;
    using System.Linq;
    using System.Net;
    using System.Security.Principal;
    using Api.Models;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using Framework;
    using Framework.Extensions;
    using Helpers;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using Pages;
    using Branch = Domain.Branch;

    [Binding]
    public class ApprovalsSteps
    {
        private ApprovalsPage ApprovalsPage => new ApprovalsPage();


        private readonly IWebClientHelper webClientHelper;
        private ICreditThresholdRepository creditThresholdRepository;
        private IBranchRepository branchRepository;
        private IUserRepository userRepository;
        private IUserNameProvider userNameProvider;

        public ApprovalsSteps()
        {
            var container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
           
            webClientHelper = container.GetInstance<IWebClientHelper>();
            creditThresholdRepository = container.GetInstance<ICreditThresholdRepository>();
            branchRepository = container.GetInstance<IBranchRepository>();
            userRepository = container.GetInstance<IUserRepository>();
            userNameProvider = container.GetInstance<IUserNameProvider>();
        }


        [Given(@"I have the following credit thresholds setup for all branches")]
        public void GivenIHaveTheFollowingCreditThresholdsSetupForAllBranches(Table table)
        {
            var branches = branchRepository.GetAllValidBranches();
            //////creditThresholdRepository.CurrentUser = "BDD";
            this.userNameProvider.ChangeUserName("BDD");
            foreach (var tableRow in table.Rows)
            {
                var creditThreshold = new CreditThreshold()
                {
                    ThresholdLevelId = int.Parse(tableRow["Level"]),
                    Threshold = decimal.Parse(tableRow["Threshold"]),
                    Branches = new Collection<Branch>(branches.ToList())
                };
                creditThresholdRepository.Save(creditThreshold);
            }
        }

        [Given(@"I am assigned to credit threshold '(.*)'")]
        public void GivenIAmAssignedToCreditThresholdLevel(string level)
        {
            var user = userRepository.GetByIdentity(WindowsIdentity.GetCurrent().Name);
            var thresholdLevel = EnumExtensions.GetValueFromDescription<ThresholdLevel>(level);
            userRepository.SetThresholdLevel(user, thresholdLevel);
        }

        [Given(@"(.*) deliveries are waiting credit approval")]
        public void SetDeliveriesToWaitingCredit(int noOfDeliveries)
        {
            for (int jobId = 1; jobId <= noOfDeliveries; jobId++)
            {
                //assign user to job
                var userJob = new UserJob()
                {
                    UserId = 1,
                    JobId = jobId
                };
                var res = webClientHelper.Post($"{Configuration.WellApiUrl}assign-user-to-job",
                    JsonConvert.SerializeObject(userJob));
                var codes = new List<HttpStatusCode>() {HttpStatusCode.Created, HttpStatusCode.OK};
                Assert.IsTrue(codes.Contains(webClientHelper.HttpWebResponse.StatusCode),
                    $"Unable to assign user to job, response: {res}");

                var deliverylineUpdate = new DeliveryLineModel()
                {
                    JobId = jobId,
                    LineNo = 1,
                    Damages = new List<DamageModel>()
                    {
                        new DamageModel()
                        {
                            DamageActionId = (int) DeliveryAction.Credit,
                            JobDetailReasonId = 0,
                            JobDetailSourceId = 0,
                            Quantity = 1
                        }
                    }
                };
                var address = $"{Configuration.WellApiUrl}DeliveryLine";
                var data = JsonConvert.SerializeObject(deliverylineUpdate);

                var response = webClientHelper.Put(address, data);

                Assert.AreEqual(HttpStatusCode.OK, webClientHelper.HttpWebResponse.StatusCode,
                    $"Unable to set delivery to credit, response: {response}");

                var confirmAddress = $"{Configuration.WellApiUrl}confirm-delivery-lines/{jobId}";
                var response2 = webClientHelper.Post(confirmAddress, "");

                Assert.AreEqual(HttpStatusCode.OK, webClientHelper.HttpWebResponse.StatusCode,
                    $"Unable to confirm credit, response: {response}");
            }
        }

        [When(@"I filter for threshold level (.*)")]
        [Then(@"I filter for threshold level (.*)")]
        public void GivenIFilterForThresholdLevel(int level)
        {
            ApprovalsPage.ThresholdRadioGroup.Click("Level" + level);
        }


        [When(@"I open the approval deliveries page")]
        public void WhenIOpenTheApprovalDeliveriesPage()
        {
            ApprovalsPage.Open();
        }

        [When(@"I open the widget page")]
        public void WhenIOpenTheWidgetPage()
        {
            ApprovalsPage.OpenAbsolute("widgets");
        }

        [When(@"I view the account info modal for approval row (.*)")]
        public void ViewAccountModal(int row)
        {
            var rows = this.ApprovalsPage.ApprovalsGrid.ReturnAllRows().ToList();
            rows[row - 1].GetItemInRowByClass("contact-info").Click();
        }

        [When(@"I click on approvals page (.*)")]
        public void WhenIClickOnExceptionDeliveryPage(int pageNo)
        {
            this.ApprovalsPage.Pager.Click(pageNo);
        }

        [Then(@"the following approval deliveries will be displayed")]
        public void ThenTheFollowingApprovalDeliveriesWillBeDisplayed(Table table)
        {
            ContainsSpecFlowTableResult result = this.ApprovalsPage.ApprovalsGrid.ContainsSpecFlowTable(table);
            Assert.That(result.HasError, Is.False, result.ErrorsDesc);
        }

        [Then(@"I can view the following account info details")]
        public void ThenICanTheFollowingAccountInfoDetails(Table table)
        {
            var modal = ApprovalsPage.AccountModal;
            AccountModalSteps.CompareModal(table, modal);
        }

        [When(@"I go back")]
        public void ThenIGoBack()
        {
            ApprovalsPage.Back();
        }
    }
}
