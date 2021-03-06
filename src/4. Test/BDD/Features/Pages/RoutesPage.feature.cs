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
    [NUnit.Framework.DescriptionAttribute("RoutesPage")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
    public partial class RoutesPageFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "RoutesPage.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "RoutesPage", "\tAs a User\r\n\tI wish to be able to view and filter route information\r\n\tso that i c" +
                    "an determine route progress", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("A user can view Route information")]
        public virtual void AUserCanViewRouteInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Route information", ((string[])(null)));
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("3 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.When("I open the routes page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "RouteDate",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table1.AddRow(new string[] {
                        "001",
                        "22",
                        "01/Jul/2016",
                        "HALL IAN",
                        "2",
                        "3",
                        "1",
                        "Not Started"});
            table1.AddRow(new string[] {
                        "006",
                        "22",
                        "01/Jul/2016",
                        "RENTON MARK",
                        "2",
                        "0",
                        "4",
                        "Not Started"});
            table1.AddRow(new string[] {
                        "011",
                        "22",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
#line 15
 testRunner.Then("The following routes will be displayed", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Routes with same route number spanning different dates once clicked only show the" +
            " correct details for that route")]
        public virtual void RoutesWithSameRouteNumberSpanningDifferentDatesOnceClickedOnlyShowTheCorrectDetailsForThatRoute()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Routes with same route number spanning different dates once clicked only show the" +
                    " correct details for that route", ((string[])(null)));
#line 21
this.ScenarioSetup(scenarioInfo);
#line 22
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 23
 testRunner.And("I have loaded the MultiDate Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.And("I have selected branches \'22\' and \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.And("20 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
 testRunner.When("I open the routes page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "RouteDate",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table2.AddRow(new string[] {
                        "001",
                        "22",
                        "Aug 1, 2016",
                        "HALL IAN",
                        "2",
                        "4",
                        "0",
                        "Not Started"});
            table2.AddRow(new string[] {
                        "011",
                        "22",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "9",
                        "0",
                        "Not Started"});
            table2.AddRow(new string[] {
                        "011",
                        "2",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "3",
                        "6",
                        "Not Started"});
            table2.AddRow(new string[] {
                        "001",
                        "22",
                        "Jul 1, 2016",
                        "HALL IAN",
                        "2",
                        "0",
                        "4",
                        "Not Started"});
            table2.AddRow(new string[] {
                        "006",
                        "22",
                        "Jul 1, 2016",
                        "RENTON MARK",
                        "2",
                        "0",
                        "4",
                        "Not Started"});
            table2.AddRow(new string[] {
                        "006",
                        "22",
                        "Jun 1, 2016",
                        "RENTON MARK",
                        "2",
                        "4",
                        "0",
                        "Not Started"});
#line 29
 testRunner.Then("The following routes will be displayed", ((string)(null)), table2, "Then ");
#line 37
 testRunner.And("The exceptions count matches the exceptions page for that route", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
 testRunner.And("The clean count matches the clean page for that route", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can filter Route information")]
        public virtual void AUserCanFilterRouteInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can filter Route information", ((string[])(null)));
#line 40
this.ScenarioSetup(scenarioInfo);
#line 41
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 42
 testRunner.And("I have loaded the MultiDate Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.And("3 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.And("I have selected branches \'22\' and \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.When("I open the routes page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 47
 testRunner.And("I filter the grid with the option \'Route\' and value \'011\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "RouteDate",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table3.AddRow(new string[] {
                        "011",
                        "22",
                        "Jul 1, 2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
            table3.AddRow(new string[] {
                        "011",
                        "2",
                        "Jul 1, 2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
#line 48
 testRunner.Then("The following routes will be displayed", ((string)(null)), table3, "Then ");
#line 52
 testRunner.When("I clear the filter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "RouteDate",
                        "Driver",
                        "NoOfDrops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table4.AddRow(new string[] {
                        "001",
                        "22",
                        "01/Aug/2016",
                        "HALL IAN",
                        "2",
                        "3",
                        "1",
                        "Not Started"});
            table4.AddRow(new string[] {
                        "011",
                        "22",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
            table4.AddRow(new string[] {
                        "011",
                        "2",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
            table4.AddRow(new string[] {
                        "006",
                        "22",
                        "01/Jun/2016",
                        "RENTON MARK",
                        "2",
                        "0",
                        "4",
                        "Not Started"});
#line 53
 testRunner.Then("The following routes will be displayed", ((string)(null)), table4, "Then ");
#line 59
 testRunner.When("I filter the grid with the option \'Branch\' and value \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "RouteDate",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table5.AddRow(new string[] {
                        "011",
                        "2",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
#line 60
 testRunner.Then("The following routes will be displayed", ((string)(null)), table5, "Then ");
#line 63
 testRunner.When("I select \'Route\' from the filter options", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 64
 testRunner.Then("the the previous filter should be cleared", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "RouteDate",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table6.AddRow(new string[] {
                        "001",
                        "22",
                        "01/Aug/2016",
                        "HALL IAN",
                        "2",
                        "3",
                        "1",
                        "Not Started"});
            table6.AddRow(new string[] {
                        "011",
                        "22",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
            table6.AddRow(new string[] {
                        "011",
                        "2",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "9",
                        "Not Started"});
            table6.AddRow(new string[] {
                        "006",
                        "22",
                        "01/Jun/2016",
                        "RENTON MARK",
                        "2",
                        "0",
                        "4",
                        "Not Started"});
#line 65
 testRunner.And("The following routes will be displayed", ((string)(null)), table6, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can view Route information and sort on route date")]
        public virtual void AUserCanViewRouteInformationAndSortOnRouteDate()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Route information and sort on route date", ((string[])(null)));
#line 72
this.ScenarioSetup(scenarioInfo);
#line 73
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 74
 testRunner.And("I have loaded the MultiDate Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
 testRunner.When("I open the routes page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "RouteDate",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table7.AddRow(new string[] {
                        "001",
                        "22",
                        "01/Aug/2016",
                        "HALL IAN",
                        "2",
                        "0",
                        "0",
                        "Not Started"});
            table7.AddRow(new string[] {
                        "011",
                        "22",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "0",
                        "Not Started"});
            table7.AddRow(new string[] {
                        "006",
                        "22",
                        "01/Jun/2016",
                        "RENTON MARK",
                        "2",
                        "0",
                        "0",
                        "Not Started"});
#line 77
 testRunner.Then("The following routes will be displayed", ((string)(null)), table7, "Then ");
#line 82
 testRunner.When("I click on the orderby Triangle image", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Route Date",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table8.AddRow(new string[] {
                        "006",
                        "22",
                        "01/Jun/2016",
                        "RENTON MARK",
                        "2",
                        "0",
                        "0",
                        "Not Started"});
            table8.AddRow(new string[] {
                        "011",
                        "22",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "0",
                        "Not Started"});
            table8.AddRow(new string[] {
                        "001",
                        "22",
                        "01/Aug/2016",
                        "HALL IAN",
                        "2",
                        "0",
                        "0",
                        "Not Started"});
#line 83
 testRunner.Then("The following routes ordered by date will be displayed in \'asc\' order", ((string)(null)), table8, "Then ");
#line 88
 testRunner.When("I click on the orderby Triangle image", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Branch",
                        "Route Date",
                        "Driver",
                        "Drops",
                        "Exceptions",
                        "Clean",
                        "Status"});
            table9.AddRow(new string[] {
                        "001",
                        "22",
                        "01/Aug/2016",
                        "HALL IAN",
                        "2",
                        "0",
                        "0",
                        "Not Started"});
            table9.AddRow(new string[] {
                        "011",
                        "22",
                        "01/Jul/2016",
                        "DUGDALE STEVEN",
                        "4",
                        "0",
                        "0",
                        "Not Started"});
            table9.AddRow(new string[] {
                        "006",
                        "22",
                        "01/Jun/2016",
                        "RENTON MARK",
                        "2",
                        "0",
                        "0",
                        "Not Started"});
#line 89
 testRunner.Then("The following routes ordered by date will be displayed in \'desc\' order", ((string)(null)), table9, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can page through Route information")]
        public virtual void AUserCanPageThroughRouteInformation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can page through Route information", ((string[])(null)));
#line 98
this.ScenarioSetup(scenarioInfo);
#line 99
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 100
 testRunner.And("I have loaded the Adam route data that has 21 lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
 testRunner.And("I have selected branch \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
 testRunner.When("I open the routes page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 103
 testRunner.Then("\'10\' rows of data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 104
 testRunner.And("I will have 3 pages of data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 105
 testRunner.When("I click on page \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 106
 testRunner.Then("\'10\' rows of data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 107
 testRunner.When("I click on page \'3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 108
 testRunner.Then("\'1\' rows of data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 109
 testRunner.When("I click on page \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 110
 testRunner.Then("\'10\' rows of data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 111
 testRunner.When("I click on page \'3\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 112
 testRunner.And("I select the first row of the route", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
 testRunner.And("I choose to view that routes clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
 testRunner.Then("the clean deliveries page will be opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 115
 testRunner.When("I click the back button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 116
 testRunner.Then("the routes page will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 117
 testRunner.And("\'1\' rows of data will be displayed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can drill into a Route to view exceptions")]
        public virtual void AUserCanDrillIntoARouteToViewExceptions()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can drill into a Route to view exceptions", ((string[])(null)));
#line 121
this.ScenarioSetup(scenarioInfo);
#line 122
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 123
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 125
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
 testRunner.When("I open the routes page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 127
 testRunner.And("I select the first row of the route", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
 testRunner.And("I choose to view that routes exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
 testRunner.Then("I can see that routes exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 130
 testRunner.And("the filter should be preset to route and route number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A user can drill into a Route to view clean deliveries")]
        public virtual void AUserCanDrillIntoARouteToViewCleanDeliveries()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can drill into a Route to view clean deliveries", ((string[])(null)));
#line 133
this.ScenarioSetup(scenarioInfo);
#line 134
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 135
 testRunner.And("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 136
 testRunner.And("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
 testRunner.When("I open the routes page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 139
 testRunner.And("I select the first row of the route", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 140
 testRunner.And("I choose to view that routes clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
 testRunner.Then("I can see that routes clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 142
 testRunner.And("the filter should be preset to route and route number", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
