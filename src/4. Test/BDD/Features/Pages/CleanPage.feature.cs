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
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
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
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Clean Delivery Information")]
        public virtual void AUserCanViewCleanDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Clean Delivery Information", ((string[])(null)));
#line 9
this.ScenarioSetup(scenarioInfo);
#line 10
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("3 deliveries have been marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName"});
            table1.AddRow(new string[] {
                        "001",
                        "22",
                        "1",
                        "94294343",
                        "49214.152",
                        "CSG - must be CF van"});
            table1.AddRow(new string[] {
                        "001",
                        "22",
                        "1",
                        "92545470",
                        "2874.033",
                        "CSG - must be CF van"});
            table1.AddRow(new string[] {
                        "001",
                        "22",
                        "2",
                        "92545470",
                        "2874.033",
                        "RVS SHOP"});
#line 15
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table1, "Then ");
#line 20
 testRunner.When("I view the account info modal for clean row 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Account name",
                        "Street",
                        "Town",
                        "Postcode",
                        "Contact name",
                        "Phone",
                        "Alt Phone",
                        "Email"});
            table2.AddRow(new string[] {
                        "CSG - must be CF van",
                        "112-114 Barrow Road",
                        "SILEBY",
                        "LE12 7LP",
                        "CSG Contact 1",
                        "01509815739",
                        "01234987654",
                        "contact@csg.com"});
#line 21
 testRunner.Then("I can the following account info details - clean", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can filter Clean Delivery information")]
        public virtual void AUserCanFilterCleanDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can filter Clean Delivery information", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line 27
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 28
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.And("I filter the clean delivery grid with the option \'Route\' and value \'006\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName"});
            table3.AddRow(new string[] {
                        "006",
                        "22",
                        "1",
                        "91156028",
                        "43362.048",
                        "WB - SHOP"});
            table3.AddRow(new string[] {
                        "006",
                        "22",
                        "1",
                        "92544765",
                        "02874.033",
                        "WB - SHOP"});
            table3.AddRow(new string[] {
                        "006",
                        "22",
                        "2",
                        "94295479",
                        "54107.000",
                        "WB - SHELL FORECOURT"});
            table3.AddRow(new string[] {
                        "006",
                        "22",
                        "2",
                        "94294985",
                        "54107.000",
                        "WB - SHELL FORECOURT"});
#line 33
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table3, "Then ");
#line 40
 testRunner.When("I filter the clean delivery grid with the option \'Invoice No\' and value \'94294343" +
                    "\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName"});
            table4.AddRow(new string[] {
                        "001",
                        "22",
                        "1",
                        "94294343",
                        "49214.152",
                        "CSG - must be CF van"});
#line 41
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table4, "Then ");
#line 44
 testRunner.When("I filter the clean delivery grid with the option \'Account\' and value \'28398.080\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName"});
            table5.AddRow(new string[] {
                        "011",
                        "22",
                        "5",
                        "92545853",
                        "28398.080",
                        "TESCO EXPRESS"});
#line 45
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table5, "Then ");
#line 48
 testRunner.When("I filter the clean delivery grid with the option \'Account Name\' and value \'WB - S" +
                    "HOP\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName"});
            table6.AddRow(new string[] {
                        "006",
                        "22",
                        "1",
                        "91156028",
                        "43362.048",
                        "WB - SHOP"});
            table6.AddRow(new string[] {
                        "006",
                        "22",
                        "1",
                        "92544765",
                        "02874.033",
                        "WB - SHOP"});
#line 49
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Clean Delivery Information and sort on updated date")]
        public virtual void AUserCanViewCleanDeliveryInformationAndSortOnUpdatedDate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Clean Delivery Information and sort on updated date", ((string[])(null)));
#line 54
this.ScenarioSetup(scenarioInfo);
#line 55
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 56
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.And("3 deliveries have been marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "DeliveryDate"});
            table7.AddRow(new string[] {
                        "001",
                        "22",
                        "1",
                        "94294343",
                        "49214.152",
                        "CSG - must be CF van",
                        "07/01/2016"});
            table7.AddRow(new string[] {
                        "001",
                        "22",
                        "1",
                        "92545470",
                        "2874.033",
                        "CSG - must be CF van",
                        "07/01/2016"});
            table7.AddRow(new string[] {
                        "001",
                        "22",
                        "2",
                        "92545470",
                        "2874.033",
                        "RVS SHOP",
                        "07/01/2016"});
#line 60
 testRunner.Then("the following clean deliveries will be displayed", ((string)(null)), table7, "Then ");
#line 65
 testRunner.When("I click on the orderby Triangle image in the clean deliveries grid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "DeliveryDate"});
            table8.AddRow(new string[] {
                        "001",
                        "22",
                        "2",
                        "92545470",
                        "2874.033",
                        "RVS SHOP",
                        "07/01/2016"});
            table8.AddRow(new string[] {
                        "001",
                        "22",
                        "1",
                        "92545470",
                        "2874.033",
                        "CSG - must be CF van",
                        "07/01/2016"});
            table8.AddRow(new string[] {
                        "001",
                        "22",
                        "1",
                        "94294343",
                        "49214.152",
                        "CSG - must be CF van",
                        "07/01/2016"});
#line 66
 testRunner.Then("The following clean deliveries ordered by date will be displayed in \'desc\' order", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through Clean Delivery information")]
        public virtual void AUserCanPageThroughCleanDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through Clean Delivery information", ((string[])(null)));
#line 72
this.ScenarioSetup(scenarioInfo);
#line 73
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 74
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 78
 testRunner.Then("\'10\' rows of clean delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 79
 testRunner.And("I will have 2 pages of clean delivery data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
 testRunner.When("I click on clean delivery page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 81
 testRunner.Then("\'7\' rows of clean delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 82
 testRunner.When("I click on clean delivery page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 83
 testRunner.Then("\'10\' rows of clean delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Clean Delivery Information with cash on delivery icons displayed")]
        public virtual void AUserCanViewCleanDeliveryInformationWithCashOnDeliveryIconsDisplayed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Clean Delivery Information with cash on delivery icons displayed", ((string[])(null)));
#line 85
this.ScenarioSetup(scenarioInfo);
#line 86
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 87
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
 testRunner.And("3 deliveries have been marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
 testRunner.And("the first \'clean\' delivery is not a cash on delivery customer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
 testRunner.When("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 92
 testRunner.Then("the cod delivery icon is not displayed in row 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
