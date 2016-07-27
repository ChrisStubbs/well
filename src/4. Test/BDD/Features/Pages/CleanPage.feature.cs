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
    [NUnit.Framework.DescriptionAttribute("CleanPage")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    public partial class CleanPageFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CleanPage.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CleanPage", "\tAs a user\r\n\tI wish to be able to view and filter clean delivery information\r\n\tso" +
                    " that I can determine which deliveries have been succesful", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("A user can view Clean Delivery Information")]
        public virtual void AUserCanViewCleanDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Clean Delivery Information", ((string[])(null)));
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("3 deliveries have been marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table1.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Complete"});
            table1.AddRow(new string[] {
                        "001",
                        "01",
                        "92874.033",
                        "2874.033",
                        "CSG - must be CF van",
                        "Complete"});
            table1.AddRow(new string[] {
                        "001",
                        "02",
                        "92874.033",
                        "2874.033",
                        "RVS SHOP",
                        "Complete"});
#line 14
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can filter Clean Delivery information")]
        public virtual void AUserCanFilterCleanDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can filter Clean Delivery information", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 21
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
 testRunner.And("I filter the clean delivery grid with the option \'Route\' and value \'006\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table2.AddRow(new string[] {
                        "006",
                        "01",
                        "943362.048",
                        "43362.048",
                        "WB - SHOP",
                        "Complete"});
            table2.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Complete"});
            table2.AddRow(new string[] {
                        "006",
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Complete"});
            table2.AddRow(new string[] {
                        "006",
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Complete"});
#line 27
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table2, "Then ");
#line 33
 testRunner.When("I filter the clean delivery grid with the option \'Drop\' and value \'03\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table3.AddRow(new string[] {
                        "011",
                        "03",
                        "954107.000",
                        "54107.000",
                        "WB - WAITROSE SHOP",
                        "Complete"});
            table3.AddRow(new string[] {
                        "011",
                        "03",
                        "954107.000",
                        "54107.000",
                        "WB - WAITROSE SHOP",
                        "Complete"});
#line 34
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table3, "Then ");
#line 38
 testRunner.When("I filter the clean delivery grid with the option \'Invoice No\' and value \'949214.1" +
                    "52\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table4.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Complete"});
#line 39
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table4, "Then ");
#line 42
 testRunner.When("I filter the clean delivery grid with the option \'Account\' and value \'28398.080\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table5.AddRow(new string[] {
                        "011",
                        "05",
                        "928398.080",
                        "28398.080",
                        "TESCO EXPRESS",
                        "Complete"});
#line 43
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table5, "Then ");
#line 46
 testRunner.When("I filter the clean delivery grid with the option \'Account Name\' and value \'WB - S" +
                    "HOP\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table6.AddRow(new string[] {
                        "006",
                        "01",
                        "943362.048",
                        "43362.048",
                        "WB - SHOP",
                        "Complete"});
            table6.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Complete"});
#line 47
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through Clean Delivery information")]
        public virtual void AUserCanPageThroughCleanDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through Clean Delivery information", ((string[])(null)));
#line 52
this.ScenarioSetup(scenarioInfo);
#line 53
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 54
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 58
 testRunner.Then("\'10\' rows of clean delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 59
 testRunner.And("I will have 2 pages of clean delivery data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
 testRunner.When("I click on clean delivery page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 61
 testRunner.Then("\'7\' rows of clean delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 62
 testRunner.When("I click on clean delivery page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
 testRunner.Then("\'10\' rows of clean delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
