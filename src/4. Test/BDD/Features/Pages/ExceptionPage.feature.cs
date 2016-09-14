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
    [NUnit.Framework.DescriptionAttribute("ExceptionPage")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
    public partial class ExceptionPageFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ExceptionPage.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ExceptionPage", "\tAs a user\r\n\tI wish to be able to view and filter exception delivery information\r" +
                    "\n\tso that I can determine\r\n\twhich deliveries have been unsuccesful", ProgrammingLanguage.CSharp, new string[] {
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
#line 9
#line 10
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("I have imported a valid Epod update file named \'ePOD_30062016_Update.xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Exception Delivery Information")]
        public virtual void AUserCanViewExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Exception Delivery Information", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 15
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 16
 testRunner.And("3 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
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
                        "02",
                        "92874.033",
                        "2874.033",
                        "RVS SHOP",
                        "Incomplete"});
            table1.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Incomplete"});
            table1.AddRow(new string[] {
                        "001",
                        "01",
                        "92874.033",
                        "2874.033",
                        "CSG - must be CF van",
                        "Incomplete"});
#line 18
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table1, "Then ");
#line 24
 testRunner.When("I view the account info modal for exception row 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
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
#line 25
 testRunner.Then("I can the following account info details", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can filter Exception Delivery information")]
        public virtual void AUserCanFilterExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can filter Exception Delivery information", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 30
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 33
 testRunner.And("I filter the exception delivery grid with the option \'Route\' and value \'006\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table3.AddRow(new string[] {
                        "006",
                        "01",
                        "943362.048",
                        "43362.048",
                        "WB - SHOP",
                        "Incomplete"});
            table3.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Incomplete"});
            table3.AddRow(new string[] {
                        "006",
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Incomplete"});
            table3.AddRow(new string[] {
                        "006",
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Incomplete"});
#line 34
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table3, "Then ");
#line 41
 testRunner.When("I filter the exception delivery grid with the option \'Invoice No\' and value \'9492" +
                    "14.152\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
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
                        "Incomplete"});
#line 42
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table4, "Then ");
#line 45
 testRunner.When("I filter the exception delivery grid with the option \'Account\' and value \'28398.0" +
                    "80\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
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
                        "Incomplete"});
#line 46
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table5, "Then ");
#line 49
 testRunner.When("I filter the exception delivery grid with the option \'Account Name\' and value \'WB" +
                    " - SHOP\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
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
                        "Incomplete"});
            table6.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Incomplete"});
#line 50
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through Exception Delivery information")]
        public virtual void AUserCanPageThroughExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through Exception Delivery information", ((string[])(null)));
#line 56
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 57
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 58
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
 testRunner.Then("\'10\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
 testRunner.And("I will have 2 pages of exception delivery data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
 testRunner.When("I click on exception delivery page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
 testRunner.Then("\'7\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 64
 testRunner.When("I click on exception delivery page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 65
 testRunner.Then("\'10\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("View exception details at lower level")]
        public virtual void ViewExceptionDetailsAtLowerLevel()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View exception details at lower level", ((string[])(null)));
#line 67
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 68
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 69
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 71
 testRunner.And("I click on exception row 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo",
                        "Product",
                        "Description",
                        "Value",
                        "InvoiceQuantity",
                        "DeliveryQuantity",
                        "DamagedQuantity",
                        "ShortQuantity"});
            table7.AddRow(new string[] {
                        "1",
                        "4237",
                        "Maltesers Tube 75g",
                        "80",
                        "0",
                        "0",
                        "0",
                        "0"});
            table7.AddRow(new string[] {
                        "2",
                        "7605",
                        "Bass Sherbet Lemons 200g",
                        "32",
                        "0",
                        "0",
                        "0",
                        "0"});
            table7.AddRow(new string[] {
                        "3",
                        "41957",
                        "Bournville Std 45g",
                        "84",
                        "0",
                        "0",
                        "0",
                        "0"});
            table7.AddRow(new string[] {
                        "4",
                        "3319",
                        "C.D.M Std 45g",
                        "125",
                        "0",
                        "0",
                        "0",
                        "0"});
            table7.AddRow(new string[] {
                        "5",
                        "9135",
                        "Wispa Duo 51g",
                        "395",
                        "0",
                        "0",
                        "0",
                        "0"});
#line 72
 testRunner.Then("I am shown the exception detail", ((string)(null)), table7, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Exception assigned to a user")]
        public virtual void ExceptionAssignedToAUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Exception assigned to a user", ((string[])(null)));
#line 80
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 81
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 82
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.Then("the user is assigned to that exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Assigned user to an exception can action it")]
        public virtual void AssignedUserToAnExceptionCanActionIt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assigned user to an exception can action it", ((string[])(null)));
#line 88
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 89
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 90
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 92
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
 testRunner.Then("the user is assigned to that exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 95
 testRunner.And("the user can action the exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
 testRunner.And("all other actions are disabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Assigned user to an exception drills to details and can update")]
        public virtual void AssignedUserToAnExceptionDrillsToDetailsAndCanUpdate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assigned user to an exception drills to details and can update", ((string[])(null)));
#line 98
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 99
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 100
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 102
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
 testRunner.And("I select the exception row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.Then("All the exception detail rows can be updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("UnAssigned user to an exception drills to details and can not update")]
        public virtual void UnAssignedUserToAnExceptionDrillsToDetailsAndCanNotUpdate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("UnAssigned user to an exception drills to details and can not update", ((string[])(null)));
#line 107
this.ScenarioSetup(scenarioInfo);
#line 9
this.FeatureBackground();
#line 108
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 109
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 111
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
 testRunner.And("I select an unassigned exception row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
 testRunner.Then("All the exception detail rows can not be updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
