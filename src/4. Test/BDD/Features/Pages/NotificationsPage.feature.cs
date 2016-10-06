﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.0.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace PH.Well.BDD.Features.Pages
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("NotificationsPage")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    public partial class NotificationsPageFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "NotificationsPage.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "NotificationsPage", "\tAs a well user\r\n\tI wish to be able to view and archive notifications \r\n\tso that " +
                    "I can take action in the ADAM system", ProgrammingLanguage.CSharp, new string[] {
                        "WebDriverFeature"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through notifications")]
        [NUnit.Framework.CategoryAttribute("mytag")]
        public virtual void AUserCanPageThroughNotifications()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through notifications", new string[] {
                        "mytag"});
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("10 deliveries have been assigned starting with job 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.And("10 notifications have been made starting with job 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.When("I navigate to the notifications page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
 testRunner.Then("I will have 4 pages of notification data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 17
 testRunner.When("I click on notification page 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then("\'1\' rows of notification data will be displayed on page 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Heading",
                        "Account",
                        "PicklistReference",
                        "InvoiceNumber",
                        "Contact",
                        "Reason"});
            table1.AddRow(new string[] {
                        "Credit failed",
                        "0/54107.000",
                        "4295479",
                        "",
                        "Tom Harris",
                        "Credit failed ADAM validation"});
#line 19
 testRunner.And("the following notifications with a rowcount of \'1\' will be displayed on page 4", ((string)(null)), table1, "And ");
#line 22
 testRunner.When("I click on notification page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Heading",
                        "Account",
                        "PicklistReference",
                        "InvoiceNumber",
                        "Contact",
                        "Reason"});
            table2.AddRow(new string[] {
                        "Credit failed",
                        "0/2874.033",
                        "2545470",
                        "",
                        "CSG Contact 1",
                        "Credit failed ADAM validation"});
            table2.AddRow(new string[] {
                        "Credit failed",
                        "0/2874.033",
                        "2545470",
                        "",
                        "GEN HOSPITAL",
                        "Credit failed ADAM validation"});
            table2.AddRow(new string[] {
                        "Credit failed",
                        "0/2874.033",
                        "2545419",
                        "",
                        "GEN HOSPITAL",
                        "Credit failed ADAM validation"});
#line 23
 testRunner.Then("the following notifications with a rowcount of \'3\' will be displayed on page 1", ((string)(null)), table2, "Then ");
#line 28
 testRunner.When("I click on notification page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
 testRunner.Then("\'3\' rows of notification data will be displayed on page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can archive a notification")]
        public virtual void AUserCanArchiveANotification()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can archive a notification", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 33
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 34
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.And("1 deliveries have been assigned starting with job 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
 testRunner.Given("1 notifications have been made starting with job 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 38
 testRunner.When("I navigate to the notifications page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.Then("I will have 1 pages of notification data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 40
 testRunner.When("I archive the notification 1 from rowcount 1 on page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "ModalTitle"});
            table3.AddRow(new string[] {
                        "Are you sure you want to archive the notification for 49214.152"});
#line 41
 testRunner.Then("I can see the following notification detail", ((string)(null)), table3, "Then ");
#line 44
 testRunner.When("I click \'Yes\' on the archive modal", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
 testRunner.Then("\'0\' rows of notification data will be displayed on page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
