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
    [NUnit.Framework.DescriptionAttribute("Resolved Delivery Page")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    public partial class ResolvedDeliveryPageFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ResolvedPage.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Resolved Delivery Page", "\tAs a user\r\n\tI wish to be able to view and filter resolved delivery information\r\n" +
                    "\tso that I can determine which deliveries have been resolved", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("A user can view Resolved Delivery Information")]
        public virtual void AUserCanViewResolvedDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Resolved Delivery Information", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("3 deliveries have been marked as Resolved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.When("I open the resolved deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "Action",
                        "Assigned"});
            table1.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Resolved",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "001",
                        "01",
                        "92874.033",
                        "2874.033",
                        "CSG - must be CF van",
                        "Resolved",
                        "",
                        ""});
            table1.AddRow(new string[] {
                        "001",
                        "02",
                        "92874.033",
                        "2874.033",
                        "RVS SHOP",
                        "Resolved",
                        "",
                        ""});
#line 13
 testRunner.Then("the following resolved deliveries will be displayed", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can filter Resolved Delivery information")]
        public virtual void AUserCanFilterResolvedDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can filter Resolved Delivery information", ((string[])(null)));
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.And("All the deliveries are marked as Resolved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.When("I open the resolved deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
 testRunner.And("I filter the resolved delivery grid with the option \'Route\' and value \'006\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "Action",
                        "Assigned"});
            table2.AddRow(new string[] {
                        "006",
                        "01",
                        "943362.048",
                        "43362.048",
                        "WB - SHOP",
                        "Resolved",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Resolved",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "006",
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Resolved",
                        "",
                        ""});
            table2.AddRow(new string[] {
                        "006",
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Resolved",
                        "",
                        ""});
#line 26
 testRunner.Then("the following resolved deliveries will be displayed", ((string)(null)), table2, "Then ");
#line 32
 testRunner.When("I filter the resolved delivery grid with the option \'Drop\' and value \'03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "Action",
                        "Assigned"});
            table3.AddRow(new string[] {
                        "011",
                        "03",
                        "954107.000",
                        "54107.000",
                        "WB - WAITROSE SHOP",
                        "Resolved",
                        "",
                        ""});
            table3.AddRow(new string[] {
                        "011",
                        "03",
                        "954107.000",
                        "54107.000",
                        "WB - WAITROSE SHOP",
                        "Resolved",
                        "",
                        ""});
#line 33
 testRunner.Then("the following resolved deliveries will be displayed", ((string)(null)), table3, "Then ");
#line 37
 testRunner.When("I filter the resolved delivery grid with the option \'Invoice No\' and value \'94921" +
                    "4.152\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "Action",
                        "Assigned"});
            table4.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Resolved",
                        "",
                        ""});
#line 38
 testRunner.Then("the following resolved deliveries will be displayed", ((string)(null)), table4, "Then ");
#line 41
 testRunner.When("I filter the resolved delivery grid with the option \'Account\' and value \'28398.08" +
                    "0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "Action",
                        "Assigned"});
            table5.AddRow(new string[] {
                        "011",
                        "05",
                        "928398.080",
                        "28398.080",
                        "TESCO EXPRESS",
                        "Resolved",
                        "",
                        ""});
#line 42
 testRunner.Then("the following resolved deliveries will be displayed", ((string)(null)), table5, "Then ");
#line 45
 testRunner.When("I filter the resolved delivery grid with the option \'Account Name\' and value \'WB " +
                    "- SHOP\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "Action",
                        "Assigned"});
            table6.AddRow(new string[] {
                        "006",
                        "01",
                        "943362.048",
                        "43362.048",
                        "WB - SHOP",
                        "Resolved",
                        "",
                        ""});
            table6.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Resolved",
                        "",
                        ""});
#line 46
 testRunner.Then("the following resolved deliveries will be displayed", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through Resolved Delivery information")]
        public virtual void AUserCanPageThroughResolvedDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through Resolved Delivery information", ((string[])(null)));
#line 51
this.ScenarioSetup(scenarioInfo);
#line 52
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 53
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.And("All the deliveries are marked as Resolved", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.When("I open the resolved deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
 testRunner.Then("\'10\' rows of resolved delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 58
 testRunner.And("I will have 2 pages of resolved delivery data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.When("I click on resolved delivery page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
 testRunner.Then("\'7\' rows of resolved delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
 testRunner.When("I click on resolved delivery page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
 testRunner.Then("\'10\' rows of resolved delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion