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
                    "\n\tso that I can determine\twhich deliveries have been unsuccesful", ProgrammingLanguage.CSharp, new string[] {
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
            table1.AddRow(new string[] {
                        "001",
                        "02",
                        "92874.033",
                        "2874.033",
                        "RVS SHOP",
                        "Incomplete"});
#line 17
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table1, "Then ");
#line 23
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
#line 24
 testRunner.Then("I can the following account info details", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can filter Exception Delivery information")]
        public virtual void AUserCanFilterExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can filter Exception Delivery information", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 29
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
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
#line 33
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table3, "Then ");
#line 40
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
#line 41
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table4, "Then ");
#line 44
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
#line 45
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table5, "Then ");
#line 48
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
#line 49
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Exception Delivery Information and sort on updated date")]
        public virtual void AUserCanViewExceptionDeliveryInformationAndSortOnUpdatedDate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Exception Delivery Information and sort on updated date", ((string[])(null)));
#line 55
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 56
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 57
 testRunner.And("3 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "LastUpdatedDateTime"});
            table7.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Incomplete",
                        "Sep 7, 2016, 1:28:16 PM"});
            table7.AddRow(new string[] {
                        "001",
                        "01",
                        "92874.033",
                        "2874.033",
                        "CSG - must be CF van",
                        "Incomplete",
                        "Sep 7, 2016, 1:30:17 PM"});
            table7.AddRow(new string[] {
                        "001",
                        "02",
                        "92874.033",
                        "2874.033",
                        "RVS SHOP",
                        "Incomplete",
                        "Sep 7, 2016, 1:27:17 PM"});
#line 59
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table7, "Then ");
#line 65
 testRunner.When("I click on the orderby Triangle image in the exceptions deliveries grid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "LastUpdatedDateTime"});
            table8.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Incomplete",
                        "Sep 7, 2016, 1:28:16 PM"});
            table8.AddRow(new string[] {
                        "001",
                        "01",
                        "92874.033",
                        "2874.033",
                        "CSG - must be CF van",
                        "Incomplete",
                        "Sep 7, 2016, 1:30:17 PM"});
            table8.AddRow(new string[] {
                        "001",
                        "02",
                        "92874.033",
                        "2874.033",
                        "RVS SHOP",
                        "Incomplete",
                        "Sep 7, 2016, 1:27:17 PM"});
#line 66
 testRunner.Then("The following exceptions ordered by date will be displayed in \'desc\' order", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through Exception Delivery information")]
        public virtual void AUserCanPageThroughExceptionDeliveryInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through Exception Delivery information", ((string[])(null)));
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
 testRunner.Then("\'10\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 82
 testRunner.And("I will have 2 pages of exception delivery data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
 testRunner.When("I click on exception delivery page 2", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
 testRunner.Then("\'7\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 85
 testRunner.When("I click on exception delivery page 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 86
 testRunner.Then("\'10\' rows of exception delivery data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("View exception details at lower level")]
        public virtual void ViewExceptionDetailsAtLowerLevel()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View exception details at lower level", ((string[])(null)));
#line 88
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 89
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 90
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 92
 testRunner.And("I click on exception row 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo",
                        "Product",
                        "Description",
                        "Value",
                        "InvoiceQuantity",
                        "DeliveryQuantity",
                        "DamagedQuantity",
                        "ShortQuantity"});
            table9.AddRow(new string[] {
                        "1",
                        "6987",
                        "Choc Teacakes Tunnock",
                        "19",
                        "1",
                        "1",
                        "0",
                        "0"});
            table9.AddRow(new string[] {
                        "2",
                        "49179",
                        "Ginger Nuts 250g",
                        "4",
                        "1",
                        "1",
                        "0",
                        "0"});
            table9.AddRow(new string[] {
                        "3",
                        "21633",
                        "Kiddies Super Mix 220gPM",
                        "3",
                        "1",
                        "1",
                        "0",
                        "0"});
            table9.AddRow(new string[] {
                        "4",
                        "4244",
                        "Milkybar Btns Giant PM",
                        "5",
                        "1",
                        "1",
                        "0",
                        "0"});
            table9.AddRow(new string[] {
                        "5",
                        "7621",
                        "Fruit Past Tube 52.5g",
                        "8",
                        "1",
                        "1",
                        "0",
                        "0"});
#line 93
 testRunner.Then("I am shown the exception detail", ((string)(null)), table9, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Exception assigned to a user")]
        public virtual void ExceptionAssignedToAUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Exception assigned to a user", ((string[])(null)));
#line 101
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 102
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 103
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 104
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 105
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
 testRunner.Then("the user is assigned to that exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Assigned user to an exception can action it")]
        public virtual void AssignedUserToAnExceptionCanActionIt()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assigned user to an exception can action it", ((string[])(null)));
#line 109
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 110
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 111
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 113
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
 testRunner.Then("the user is assigned to that exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 116
 testRunner.And("the user can action the exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
 testRunner.And("all other actions are disabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Assigned user to an exception drills to details and can update")]
        public virtual void AssignedUserToAnExceptionDrillsToDetailsAndCanUpdate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Assigned user to an exception drills to details and can update", ((string[])(null)));
#line 119
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 120
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 121
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 123
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
 testRunner.And("I select the exception row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
 testRunner.Then("All the exception detail rows can be updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("UnAssigned user to an exception drills to details and can not update")]
        public virtual void UnAssignedUserToAnExceptionDrillsToDetailsAndCanNotUpdate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("UnAssigned user to an exception drills to details and can not update", ((string[])(null)));
#line 128
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 129
 testRunner.Given("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 130
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 132
 testRunner.And("I select the assigned link on the first row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
 testRunner.And("I select a user to assign", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
 testRunner.And("I select an unassigned exception row", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
 testRunner.Then("All the exception detail rows can not be updated", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user cannot view Exception Delivery Information without a valid invoice number")]
        public virtual void AUserCannotViewExceptionDeliveryInformationWithoutAValidInvoiceNumber()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user cannot view Exception Delivery Information without a valid invoice number", ((string[])(null)));
#line 137
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 138
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 139
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
 testRunner.And("I have imported a valid Epod update file named \'ePOD__MissingInvoiceNumbers.xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
 testRunner.And("I have selected branch 22", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 143
 testRunner.Then("there are 0 exception deliveries will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 144
 testRunner.Given("3 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 145
 testRunner.When("valid invoice numbers are assigned to jobs", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 146
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table10.AddRow(new string[] {
                        "001",
                        "01",
                        "949214.152",
                        "49214.152",
                        "CSG - must be CF van",
                        "Incomplete"});
            table10.AddRow(new string[] {
                        "001",
                        "01",
                        "92874.033",
                        "2874.033",
                        "CSG - must be CF van",
                        "Incomplete"});
            table10.AddRow(new string[] {
                        "001",
                        "02",
                        "92874.033",
                        "2874.033",
                        "RVS SHOP",
                        "Incomplete"});
#line 147
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table10, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
