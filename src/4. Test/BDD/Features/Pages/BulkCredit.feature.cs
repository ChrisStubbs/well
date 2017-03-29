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
    [NUnit.Framework.DescriptionAttribute("Bulk Credit Feature")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
    public partial class BulkCreditFeatureFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "BulkCredit.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Bulk Credit Feature", "\tAs a user I need to be able to bulk credit Delievry Exceptions", ProgrammingLanguage.CSharp, new string[] {
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
#line 6
#line 7
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 8
 testRunner.And("I have selected branch \'55\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.And("I import the route file \'ROUTE_PLYM_BulkCredit.xml\' into the well", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("I have loaded the order file \'ORDER_PLY_BulkCredit.xml\' into the well", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Filename"});
            table1.AddRow(new string[] {
                        "ePOD__BulkCredit1.xml"});
            table1.AddRow(new string[] {
                        "ePOD__BulkCredit2.xml"});
            table1.AddRow(new string[] {
                        "ePOD__BulkCredit3.xml"});
#line 11
 testRunner.And("I have imported the following valid Epod files", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Value",
                        "Branch"});
            table2.AddRow(new string[] {
                        "1",
                        "1000",
                        "55"});
            table2.AddRow(new string[] {
                        "2",
                        "30",
                        "55"});
            table2.AddRow(new string[] {
                        "3",
                        "10",
                        "55"});
#line 16
 testRunner.And("I have the following credit threshold levels set in the database", ((string)(null)), table2, "And ");
#line 21
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "CreditValue",
                        "Status",
                        "TBA"});
            table3.AddRow(new string[] {
                        "111",
                        "55",
                        "1",
                        "4800011",
                        "45649.000",
                        "SHELL - TRERULEFOOT",
                        "5.4",
                        "Incomplete",
                        "0"});
            table3.AddRow(new string[] {
                        "111",
                        "55",
                        "3",
                        "4800013",
                        "37432.000",
                        "SHELL - KINGSLEY VIL",
                        "5.4",
                        "Incomplete",
                        "0"});
            table3.AddRow(new string[] {
                        "111",
                        "55",
                        "4",
                        "2845610",
                        "47020.053",
                        "COSTCUTTER",
                        "158.46",
                        "Incomplete",
                        "0"});
            table3.AddRow(new string[] {
                        "111",
                        "55",
                        "4",
                        "4800016",
                        "47663.040",
                        "COSTCUTTER",
                        "25.32",
                        "Incomplete",
                        "0"});
#line 22
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table3, "Then ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user with sufficient credit threshold can bulk credit multiple delivery excepti" +
            "ons and upon")]
        public virtual void AUserWithSufficientCreditThresholdCanBulkCreditMultipleDeliveryExceptionsAndUpon()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user with sufficient credit threshold can bulk credit multiple delivery excepti" +
                    "ons and upon", ((string[])(null)));
#line 30
 this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 32
 testRunner.Given("I am assigned to credit threshold \'Level 1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo"});
            table4.AddRow(new string[] {
                        "1"});
            table4.AddRow(new string[] {
                        "2"});
            table4.AddRow(new string[] {
                        "3"});
            table4.AddRow(new string[] {
                        "4"});
#line 33
 testRunner.And("I assign the following exception lines to myself", ((string)(null)), table4, "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo"});
            table5.AddRow(new string[] {
                        "1"});
            table5.AddRow(new string[] {
                        "2"});
            table5.AddRow(new string[] {
                        "3"});
            table5.AddRow(new string[] {
                        "4"});
#line 39
 testRunner.And("I click the credit checkbox on the following lines", ((string)(null)), table5, "And ");
#line 45
 testRunner.When("I click the Bulk Credit button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 46
 testRunner.And("Select the Sources as \'Not Defined\' and reason as \'Not Defined\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.And("I click the bulk modal Confirm button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.Then("the exception deliveries page will show No exceptions found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 49
 testRunner.When("I open the resolved deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table6.AddRow(new string[] {
                        "111",
                        "55",
                        "1",
                        "4800011",
                        "45649.000",
                        "SHELL - TRERULEFOOT",
                        "Resolved"});
            table6.AddRow(new string[] {
                        "111",
                        "55",
                        "3",
                        "4800013",
                        "37432.000",
                        "SHELL - KINGSLEY VIL",
                        "Resolved"});
            table6.AddRow(new string[] {
                        "111",
                        "55",
                        "4",
                        "2845610",
                        "47020.053",
                        "COSTCUTTER",
                        "Resolved"});
            table6.AddRow(new string[] {
                        "111",
                        "55",
                        "4",
                        "4800016",
                        "47663.040",
                        "COSTCUTTER",
                        "Resolved"});
#line 50
 testRunner.Then("the following resolved deliveries grid will be displayed", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user with insufficient credit threshold can bulk credit multiple delivery excep" +
            "tions and upon")]
        public virtual void AUserWithInsufficientCreditThresholdCanBulkCreditMultipleDeliveryExceptionsAndUpon()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user with insufficient credit threshold can bulk credit multiple delivery excep" +
                    "tions and upon", ((string[])(null)));
#line 57
 this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 59
 testRunner.Given("I am assigned to credit threshold \'Level 2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo"});
            table7.AddRow(new string[] {
                        "1"});
            table7.AddRow(new string[] {
                        "2"});
            table7.AddRow(new string[] {
                        "3"});
            table7.AddRow(new string[] {
                        "4"});
#line 60
 testRunner.And("I assign the following exception lines to myself", ((string)(null)), table7, "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo"});
            table8.AddRow(new string[] {
                        "1"});
            table8.AddRow(new string[] {
                        "2"});
            table8.AddRow(new string[] {
                        "3"});
            table8.AddRow(new string[] {
                        "4"});
#line 66
 testRunner.And("I click the credit checkbox on the following lines", ((string)(null)), table8, "And ");
#line 72
 testRunner.When("I click the Bulk Credit button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 73
 testRunner.And("Select the Sources as \'Not Defined\' and reason as \'Not Defined\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.And("I click the bulk modal Confirm button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.Then("the exception deliveries page will show No exceptions found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
 testRunner.When("I open the resolved deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status"});
            table9.AddRow(new string[] {
                        "111",
                        "55",
                        "1",
                        "4800011",
                        "45649.000",
                        "SHELL - TRERULEFOOT",
                        "Resolved"});
            table9.AddRow(new string[] {
                        "111",
                        "55",
                        "3",
                        "4800013",
                        "37432.000",
                        "SHELL - KINGSLEY VIL",
                        "Resolved"});
            table9.AddRow(new string[] {
                        "111",
                        "55",
                        "4",
                        "4800016",
                        "47663.040",
                        "COSTCUTTER",
                        "Resolved"});
#line 77
 testRunner.Then("the following resolved deliveries grid will be displayed", ((string)(null)), table9, "Then ");
#line 82
 testRunner.When("I open the approval deliveries page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "CreditValue",
                        "Threshold",
                        "Assigned"});
            table10.AddRow(new string[] {
                        "111",
                        "55",
                        "4",
                        "2845610",
                        "47020.053",
                        "COSTCUTTER",
                        "158.46",
                        "Level 1",
                        "Unallocated"});
#line 83
 testRunner.Then("the following approval deliveries will be displayed", ((string)(null)), table10, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user will be unable to credit a delivery where a delivery line has an action on" +
            " it.")]
        public virtual void AUserWillBeUnableToCreditADeliveryWhereADeliveryLineHasAnActionOnIt_()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user will be unable to credit a delivery where a delivery line has an action on" +
                    " it.", ((string[])(null)));
#line 88
 this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 89
 testRunner.Given("I am assigned to credit threshold \'Level 1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo"});
            table11.AddRow(new string[] {
                        "1"});
#line 90
 testRunner.And("I assign the following exception lines to myself", ((string)(null)), table11, "And ");
#line 93
 testRunner.Then("I click on exception row 1", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 94
 testRunner.And("I open the clean delivery \'7\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
 testRunner.And("I click on the first delivery line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
 testRunner.And("I enter a short quantity of \'5\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
 testRunner.And("I select a short source of \'Checker\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
 testRunner.And("I select a short reason of \'Minimum Drop Charge\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
 testRunner.And("I select a short action of \'Credit\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
 testRunner.Then("I save the delivery line updates", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 101
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 102
 testRunner.Then("The credit check box on line 1 is disabled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion