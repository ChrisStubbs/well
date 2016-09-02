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
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Exception Delivery Information")]
        public virtual void AUserCanViewExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Exception Delivery Information", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 14
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
 testRunner.And("3 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
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
                        "92874.033",
                        "2874.033",
                        "CSG - must be CF van",
                        "Incomplete"});
            table1.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Incomplete"});
#line 17
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can filter Exception Delivery information")]
        public virtual void AUserCanFilterExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can filter Exception Delivery information", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 25
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.And("I filter the exception delivery grid with the option \'Route\' and value \'006\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Incomplete"});
            table2.AddRow(new string[] {
                        "006",
                        "02",
                        "954107.000",
                        "54107.000",
                        "WB - SHELL FORECOURT",
                        "Incomplete"});
            table2.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Incomplete"});
            table2.AddRow(new string[] {
                        "006",
                        "01",
                        "943362.048",
                        "43362.048",
                        "WB - SHOP",
                        "Incomplete"});
#line 29
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table2, "Then ");
#line 37
 testRunner.When("I filter the exception delivery grid with the option \'Invoice No\' and value \'9492" +
                    "14.152\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table3.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Incomplete"});
#line 38
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table3, "Then ");
#line 41
 testRunner.When("I filter the exception delivery grid with the option \'Account\' and value \'28398.0" +
                    "80\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table4.AddRow(new string[] {
                        "011",
                        "05",
                        "928398.080",
                        "28398.080",
                        "TESCO EXPRESS",
                        "Incomplete"});
#line 42
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table4, "Then ");
#line 45
 testRunner.When("I filter the exception delivery grid with the option \'Account Name\' and value \'WB" +
                    " - SHOP\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table5.AddRow(new string[] {
                        "006",
                        "01",
                        "92874.033",
                        "2874.033",
                        "WB - SHOP",
                        "Incomplete"});
            table5.AddRow(new string[] {
                        "006",
                        "01",
                        "943362.048",
                        "43362.048",
                        "WB - SHOP",
                        "Incomplete"});
#line 46
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table5, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through Exception Delivery information")]
        public virtual void AUserCanPageThroughExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through Exception Delivery information", ((string[])(null)));
#line 53
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 54
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 55
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
 testRunner.Then("\'10\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 58
 testRunner.And("I will have 2 pages of exception delivery data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.When("I click on exception delivery page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 60
 testRunner.Then("\'7\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
 testRunner.When("I click on exception delivery page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 62
 testRunner.Then("\'10\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("View exception details at lower level")]
        public virtual void ViewExceptionDetailsAtLowerLevel()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View exception details at lower level", ((string[])(null)));
#line 64
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 65
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 66
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 68
 testRunner.And("I click on a exception row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo",
                        "Product",
                        "Description",
                        "Value",
                        "InvoiceQuantity",
                        "DeliveryQuantity",
                        "DamagedQuantity",
                        "ShortQuantity"});
            table6.AddRow(new string[] {
                        "1",
                        "4237",
                        "Maltesers Tube 75g",
                        "80",
                        "0",
                        "0",
                        "0",
                        "0"});
            table6.AddRow(new string[] {
                        "2",
                        "7605",
                        "Bass Sherbet Lemons 200g",
                        "32",
                        "0",
                        "0",
                        "0",
                        "0"});
            table6.AddRow(new string[] {
                        "3",
                        "41957",
                        "Bournville Std 45g",
                        "84",
                        "0",
                        "0",
                        "0",
                        "0"});
            table6.AddRow(new string[] {
                        "4",
                        "3319",
                        "C.D.M Std 45g",
                        "125",
                        "0",
                        "0",
                        "0",
                        "0"});
            table6.AddRow(new string[] {
                        "5",
                        "9135",
                        "Wispa Duo 51g",
                        "395",
                        "0",
                        "0",
                        "0",
                        "0"});
#line 69
 testRunner.Then("I am shown the exception detail", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Exception assigned to a user")]
        public virtual void ExceptionAssignedToAUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Exception assigned to a user", ((string[])(null)));
#line 77
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 78
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 81
 testRunner.And("I select the assigned link", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
 testRunner.Then("the user is assigned to that exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
