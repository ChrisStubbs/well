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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Approvals Page", "\tAs a user\r\n\tI wish to be able to view and filter deliveries waiting credit appro" +
                    "val\r\n\tso that I can find and approve credits", ProgrammingLanguage.CSharp, new string[] {
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
 testRunner.And("I have loaded the MultiDate Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold"});
            table1.AddRow(new string[] {
                        "1",
                        "500"});
            table1.AddRow(new string[] {
                        "2",
                        "30"});
            table1.AddRow(new string[] {
                        "3",
                        "5"});
#line 11
 testRunner.And("I have the following credit thresholds setup for all branches", ((string)(null)), table1, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Approvals Browsing and Paging")]
        public virtual void ApprovalsBrowsingAndPaging()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Approvals Browsing and Paging", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 18
 testRunner.Given("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
 testRunner.And("I am assigned to credit threshold \'Level 3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.And("11 deliveries are waiting credit approval", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.When("I open the approval deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "CreditValue",
                        "Threshold",
                        "Assigned"});
            table2.AddRow(new string[] {
                        "001",
                        "1",
                        "976549",
                        "49214.152",
                        "CSG - must be CF van",
                        "39.95",
                        "Level 1",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "001",
                        "1",
                        "976549",
                        "2874.033",
                        "CSG - must be CF van",
                        "22.41",
                        "Level 2",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "001",
                        "2",
                        "976541",
                        "2874.033",
                        "RVS SHOP",
                        "39.95",
                        "Level 1",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "001",
                        "2",
                        "976542",
                        "02874.033",
                        "RVS SHOP",
                        "19.23",
                        "Level 2",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "006",
                        "1",
                        "123123123",
                        "43362.048",
                        "WB - SHOP",
                        "24.72",
                        "Level 2",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "006",
                        "1",
                        "223123123",
                        "02874.033",
                        "WB - SHOP",
                        "80",
                        "Level 1",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "006",
                        "2",
                        "323123123",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "7.32",
                        "Level 2",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "006",
                        "2",
                        "423123123",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "176.7",
                        "Level 1",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "011",
                        "1",
                        "976549",
                        "43362.048",
                        "CSG - COSTCUTTER",
                        "24.72",
                        "Level 2",
                        "Unallocated"});
            table2.AddRow(new string[] {
                        "011",
                        "1",
                        "976549",
                        "02874.033",
                        "CSG - COSTCUTTER",
                        "80",
                        "Level 1",
                        "Unallocated"});
#line 22
 testRunner.Then("the following approval deliveries will be displayed", ((string)(null)), table2, "Then ");
#line 34
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
#line 35
 testRunner.Then("I can view the following account info details", ((string)(null)), table3, "Then ");
#line 38
 testRunner.When("I click on approvals page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "CreditValue",
                        "Threshold",
                        "Assigned"});
            table4.AddRow(new string[] {
                        "011",
                        "2",
                        "976549",
                        "54107.000",
                        "TESCO - EXPRESS",
                        "7.32",
                        "Level 2",
                        "Unallocated"});
#line 39
 testRunner.Then("the following approval deliveries will be displayed", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Threshold Filtering")]
        public virtual void ThresholdFiltering()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Threshold Filtering", ((string[])(null)));
#line 43
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 44
 testRunner.Given("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 45
 testRunner.And("I am assigned to credit threshold \'Level 3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.And("11 deliveries are waiting credit approval", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.When("I open the approval deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 48
 testRunner.And("I filter for threshold level 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "CreditValue",
                        "Threshold",
                        "Assigned"});
            table5.AddRow(new string[] {
                        "001",
                        "1",
                        "976549",
                        "02874.033",
                        "CSG - must be CF van",
                        "22.41",
                        "Level 2",
                        "Unallocated"});
            table5.AddRow(new string[] {
                        "001",
                        "2",
                        "976542",
                        "02874.033",
                        "RVS SHOP",
                        "19.23",
                        "Level 2",
                        "Unallocated"});
            table5.AddRow(new string[] {
                        "006",
                        "1",
                        "123123123",
                        "43362.048",
                        "WB - SHOP",
                        "24.72",
                        "Level 2",
                        "Unallocated"});
            table5.AddRow(new string[] {
                        "006",
                        "2",
                        "323123123",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "7.32",
                        "Level 2",
                        "Unallocated"});
            table5.AddRow(new string[] {
                        "011",
                        "1",
                        "976549",
                        "43362.048",
                        "CSG - COSTCUTTER",
                        "24.72",
                        "Level 2",
                        "Unallocated"});
            table5.AddRow(new string[] {
                        "011",
                        "2",
                        "976549",
                        "54107.000",
                        "TESCO - EXPRESS",
                        "7.32",
                        "Level 2",
                        "Unallocated"});
#line 49
 testRunner.Then("the following approval deliveries will be displayed", ((string)(null)), table5, "Then ");
#line 57
 testRunner.And("I filter for threshold level 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "CreditValue",
                        "Threshold",
                        "Assigned"});
            table6.AddRow(new string[] {
                        "001",
                        "1",
                        "976549",
                        "49214.152",
                        "CSG - must be CF van",
                        "39.95",
                        "Level 1",
                        "Unallocated"});
            table6.AddRow(new string[] {
                        "001",
                        "2",
                        "976541",
                        "02874.033",
                        "RVS SHOP",
                        "39.95",
                        "Level 1",
                        "Unallocated"});
            table6.AddRow(new string[] {
                        "006",
                        "1",
                        "223123123",
                        "02874.033",
                        "WB - SHOP",
                        "80",
                        "Level 1",
                        "Unallocated"});
            table6.AddRow(new string[] {
                        "006",
                        "2",
                        "423123123",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "176.7",
                        "Level 1",
                        "Unallocated"});
            table6.AddRow(new string[] {
                        "011",
                        "1",
                        "976549",
                        "02874.033",
                        "CSG - COSTCUTTER",
                        "80",
                        "Level 1",
                        "Unallocated"});
#line 58
 testRunner.Then("the following approval deliveries will be displayed", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
