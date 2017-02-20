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
    [NUnit.Framework.DescriptionAttribute("Approvals Page")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
    public partial class ApprovalsPageFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ApprovalsPage.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Approvals Page", "\tAs a user\r\n\tI wish to be able to view and filter credits waiting approval\r\n\tso t" +
                    "hat I can find and approve credits", ProgrammingLanguage.CSharp, new string[] {
                        "WebDriverFeature",
                        "RoleSuperUser"});
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
        
        public virtual void FeatureBackground()
        {
#line 8
#line 9
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("I have imported a valid Epod update file named \'ePOD_30062016_Update.xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold"});
            table1.AddRow(new string[] {
                        "1",
                        "200"});
            table1.AddRow(new string[] {
                        "2",
                        "100"});
            table1.AddRow(new string[] {
                        "3",
                        "10"});
#line 12
 testRunner.And("I have the following credit thresholds setup for all branches", ((string)(null)), table1, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Deliveries waiting credit approval")]
        public virtual void AUserCanViewDeliveriesWaitingCreditApproval()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Deliveries waiting credit approval", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 20
 testRunner.Given("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
 testRunner.And("I am assigned to credit threshold \'Level 3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.And("3 deliveries are waiting credit approval", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.When("I open the approval deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "CreditValue",
                        "Status",
                        "Threshold"});
            table2.AddRow(new string[] {
                        "001",
                        "1",
                        "1787878",
                        "49214.152",
                        "CSG - must be CF van",
                        "199.75",
                        "Incomplete",
                        "Level 1"});
            table2.AddRow(new string[] {
                        "001",
                        "1",
                        "976549",
                        "2874.033",
                        "CSG - must be CF van",
                        "22.41",
                        "Incomplete",
                        "Level 2"});
            table2.AddRow(new string[] {
                        "001",
                        "2",
                        "976541",
                        "2874.033",
                        "RVS SHOP",
                        "39.95",
                        "Incomplete",
                        "Level 2"});
#line 24
 testRunner.Then("the following approval deliveries will be displayed", ((string)(null)), table2, "Then ");
#line 29
 testRunner.When("I view the account info modal for approval row 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account name",
                        "Street",
                        "Town",
                        "Postcode",
                        "Contact name",
                        "Phone",
                        "Alt Phone",
                        "Email"});
            table3.AddRow(new string[] {
                        "CSG - must be CF van",
                        "112-114 Barrow Road",
                        "SILEBY",
                        "LE12 7LP",
                        "CSG Contact 1",
                        "01509815739",
                        "01234987654",
                        "contact@csg.com"});
#line 30
 testRunner.Then("I can view the following account info details", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
