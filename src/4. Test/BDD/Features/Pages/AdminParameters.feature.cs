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
    [NUnit.Framework.DescriptionAttribute("Admininistration Parameters")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
    public partial class AdmininistrationParametersFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AdminParameters.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Admininistration Parameters", @"	In order to parameterise the well
	As a user
	I want to be able to set seasonal dates so that clean deliveries take these dates into account when getting cleared from the well
	And I want to be able to set credit threshold per branch
	And I want to be able to set the time clean deliveries are cleaned from the well
	And I want to be able to set widget warning levels per branch", ProgrammingLanguage.CSharp, new string[] {
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
        [NUnit.Framework.DescriptionAttribute("Seasonal dates add new")]
        public virtual void SeasonalDatesAddNew()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Seasonal dates add new", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 13
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate"});
            table1.AddRow(new string[] {
                        "New Year",
                        "24/12/2016",
                        "04/01/2017"});
#line 14
 testRunner.When("I add a seasonal date", ((string)(null)), table1, "When ");
#line 17
 testRunner.And("all branches are selected for the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.And("I save the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate",
                        "Branches"});
            table2.AddRow(new string[] {
                        "New Year",
                        "24/12/2016",
                        "04/01/2017",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 19
 testRunner.Then("the seasonal date is saved", ((string)(null)), table2, "Then ");
#line 22
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate",
                        "Branches"});
            table3.AddRow(new string[] {
                        "New Year",
                        "24/12/2016",
                        "04/01/2017",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 23
 testRunner.And("the seasonal date is saved", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Seasonal dates remove")]
        public virtual void SeasonalDatesRemove()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Seasonal dates remove", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 28
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate"});
            table4.AddRow(new string[] {
                        "New Year",
                        "24/12/2016",
                        "04/01/2017"});
#line 30
 testRunner.When("I add a seasonal date", ((string)(null)), table4, "When ");
#line 33
 testRunner.And("all branches are selected for the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.And("I save the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate",
                        "Branches"});
            table5.AddRow(new string[] {
                        "New Year",
                        "24/12/2016",
                        "04/01/2017",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 35
 testRunner.Then("the seasonal date is saved", ((string)(null)), table5, "Then ");
#line 38
 testRunner.When("I remove the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 39
 testRunner.Then("it is removed from the seasonal date grid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Seasonal dates edit")]
        public virtual void SeasonalDatesEdit()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Seasonal dates edit", ((string[])(null)));
#line 41
this.ScenarioSetup(scenarioInfo);
#line 42
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 43
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate"});
            table6.AddRow(new string[] {
                        "New Year",
                        "24/12/2016",
                        "04/01/2017"});
#line 44
 testRunner.When("I add a seasonal date", ((string)(null)), table6, "When ");
#line 47
 testRunner.And("all branches are selected for the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.And("I save the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate",
                        "Branches"});
            table7.AddRow(new string[] {
                        "New Year",
                        "24/12/2016",
                        "04/01/2017",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 49
 testRunner.Then("the seasonal date is saved", ((string)(null)), table7, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate"});
            table8.AddRow(new string[] {
                        "New Years Eve",
                        "25/12/2016",
                        "02/01/2017"});
#line 52
 testRunner.When("I edit a seasonal date", ((string)(null)), table8, "When ");
#line 55
 testRunner.And("I update the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description",
                        "FromDate",
                        "ToDate",
                        "Branches"});
            table9.AddRow(new string[] {
                        "New Years Eve",
                        "25/12/2016",
                        "02/01/2017",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 56
 testRunner.Then("the seasonal date is updated with id \'2\'", ((string)(null)), table9, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Credit threshold add new")]
        public virtual void CreditThresholdAddNew()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Credit threshold add new", ((string[])(null)));
#line 60
this.ScenarioSetup(scenarioInfo);
#line 61
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 62
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold"});
            table10.AddRow(new string[] {
                        "Level1",
                        "1000"});
#line 63
 testRunner.When("I add a credit threshold", ((string)(null)), table10, "When ");
#line 66
 testRunner.And("all branches are selected for the credit threshold", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.And("I save the credit threshold", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold",
                        "Branches"});
            table11.AddRow(new string[] {
                        "Level 1",
                        "1000",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 68
 testRunner.Then("the credit threshold is saved", ((string)(null)), table11, "Then ");
#line 71
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
 testRunner.When("I select the credit threshold tab", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold",
                        "Branches"});
            table12.AddRow(new string[] {
                        "Level 1",
                        "1000",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 73
 testRunner.Then("the credit threshold is saved", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Credit threshold remove")]
        public virtual void CreditThresholdRemove()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Credit threshold remove", ((string[])(null)));
#line 77
this.ScenarioSetup(scenarioInfo);
#line 78
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 79
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold"});
            table13.AddRow(new string[] {
                        "Level1",
                        "1000"});
#line 80
 testRunner.When("I add a credit threshold", ((string)(null)), table13, "When ");
#line 83
 testRunner.And("all branches are selected for the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
 testRunner.And("I save the credit threshold", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold",
                        "Branches"});
            table14.AddRow(new string[] {
                        "Level 1",
                        "1000",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 85
 testRunner.Then("the credit threshold is saved", ((string)(null)), table14, "Then ");
#line 88
 testRunner.When("I remove the credit threshold", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 89
 testRunner.Then("it is removed from the credit threshold grid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Credit threshold edit")]
        public virtual void CreditThresholdEdit()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Credit threshold edit", ((string[])(null)));
#line 91
this.ScenarioSetup(scenarioInfo);
#line 92
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 93
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold"});
            table15.AddRow(new string[] {
                        "Level1",
                        "1000"});
#line 94
 testRunner.When("I add a credit threshold", ((string)(null)), table15, "When ");
#line 97
 testRunner.And("all branches are selected for the seasonal date", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
 testRunner.And("I save the credit threshold", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold",
                        "Branches"});
            table16.AddRow(new string[] {
                        "Level 1",
                        "1000",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 99
 testRunner.Then("the credit threshold is saved", ((string)(null)), table16, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Threshold"});
            table17.AddRow(new string[] {
                        "2000"});
#line 102
 testRunner.When("I edit a credit threshold", ((string)(null)), table17, "When ");
#line 105
 testRunner.And("I update the credit threshold", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Threshold",
                        "Branches"});
            table18.AddRow(new string[] {
                        "Level 1",
                        "2000",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 106
 testRunner.Then("the credit threshold is updated with id \'2\'", ((string)(null)), table18, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Clean parameter add new")]
        public virtual void CleanParameterAddNew()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Clean parameter add new", ((string[])(null)));
#line 110
this.ScenarioSetup(scenarioInfo);
#line 111
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 112
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days"});
            table19.AddRow(new string[] {
                        "1"});
#line 113
 testRunner.When("I add a clean parameter", ((string)(null)), table19, "When ");
#line 116
 testRunner.And("all branches are selected for the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
 testRunner.And("I save the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Branches"});
            table20.AddRow(new string[] {
                        "1",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 118
 testRunner.Then("the clean parameter is saved", ((string)(null)), table20, "Then ");
#line 121
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 122
 testRunner.When("I select the clean parameter tab", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Branches"});
            table21.AddRow(new string[] {
                        "1",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 123
 testRunner.Then("the clean parameter is saved", ((string)(null)), table21, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Clean parameters remove")]
        public virtual void CleanParametersRemove()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Clean parameters remove", ((string[])(null)));
#line 127
this.ScenarioSetup(scenarioInfo);
#line 128
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 129
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days"});
            table22.AddRow(new string[] {
                        "1"});
#line 130
 testRunner.When("I add a clean parameter", ((string)(null)), table22, "When ");
#line 133
 testRunner.And("all branches are selected for the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
 testRunner.And("I save the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Branches"});
            table23.AddRow(new string[] {
                        "1",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 135
 testRunner.Then("the clean parameter is saved", ((string)(null)), table23, "Then ");
#line 138
 testRunner.When("I remove the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 139
 testRunner.Then("it is removed from the clean parameter grid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Clean parameters edit")]
        public virtual void CleanParametersEdit()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Clean parameters edit", ((string[])(null)));
#line 141
this.ScenarioSetup(scenarioInfo);
#line 142
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 143
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days"});
            table24.AddRow(new string[] {
                        "1"});
#line 144
 testRunner.When("I add a clean parameter", ((string)(null)), table24, "When ");
#line 147
 testRunner.And("all branches are selected for the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 148
 testRunner.And("I save the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Branches"});
            table25.AddRow(new string[] {
                        "1",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 149
 testRunner.Then("the clean parameter is saved", ((string)(null)), table25, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days"});
            table26.AddRow(new string[] {
                        "2"});
#line 152
 testRunner.When("I edit a clean parameter", ((string)(null)), table26, "When ");
#line 155
 testRunner.And("I update the clean parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "Days",
                        "Branches"});
            table27.AddRow(new string[] {
                        "2",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 156
 testRunner.Then("the clean parameter is updated with id \'2\'", ((string)(null)), table27, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Widget warning parameter add new")]
        public virtual void WidgetWarningParameterAddNew()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Widget warning parameter add new", ((string[])(null)));
#line 160
this.ScenarioSetup(scenarioInfo);
#line 161
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 162
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table28 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Description"});
            table28.AddRow(new string[] {
                        "5",
                        "Exceptions",
                        "\'Test\'"});
#line 163
 testRunner.When("I add a widget warning parameter", ((string)(null)), table28, "When ");
#line 166
 testRunner.And("all branches are selected for the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 167
 testRunner.And("I save the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table29 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Branches"});
            table29.AddRow(new string[] {
                        "5",
                        "Exceptions",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 168
 testRunner.Then("the widget warning parameter is saved", ((string)(null)), table29, "Then ");
#line 171
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 172
 testRunner.When("I select the widget warning tab", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table30 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Branches"});
            table30.AddRow(new string[] {
                        "5",
                        "Exceptions",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 173
 testRunner.Then("the widget warning parameter is saved", ((string)(null)), table30, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Widget warning parameter remove")]
        public virtual void WidgetWarningParameterRemove()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Widget warning parameter remove", ((string[])(null)));
#line 177
this.ScenarioSetup(scenarioInfo);
#line 178
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 179
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table31 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Description"});
            table31.AddRow(new string[] {
                        "5",
                        "Exceptions",
                        "\'Test\'"});
#line 180
 testRunner.When("I add a widget warning parameter", ((string)(null)), table31, "When ");
#line 183
 testRunner.And("all branches are selected for the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 184
 testRunner.And("I save the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table32 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Branches"});
            table32.AddRow(new string[] {
                        "5",
                        "Exceptions",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 185
 testRunner.Then("the widget warning parameter is saved", ((string)(null)), table32, "Then ");
#line 188
 testRunner.When("I remove the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 189
 testRunner.Then("it is removed from the widget warning grid", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Widget warning parameter edit")]
        public virtual void WidgetWarningParameterEdit()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Widget warning parameter edit", ((string[])(null)));
#line 191
this.ScenarioSetup(scenarioInfo);
#line 192
 testRunner.Given("I have a clean database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 193
 testRunner.And("I navigate to the branch parameters page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table33 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Description"});
            table33.AddRow(new string[] {
                        "5",
                        "Exceptions",
                        "\'Test\'"});
#line 194
 testRunner.When("I add a widget warning parameter", ((string)(null)), table33, "When ");
#line 197
 testRunner.And("all branches are selected for the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 198
 testRunner.And("I save the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table34 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Branches"});
            table34.AddRow(new string[] {
                        "5",
                        "Exceptions",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 199
 testRunner.Then("the widget warning parameter is saved", ((string)(null)), table34, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table35 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Widget",
                        "Branches"});
            table35.AddRow(new string[] {
                        "2",
                        "Exceptions",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 202
 testRunner.When("I edit a widget warning parameter", ((string)(null)), table35, "When ");
#line 205
 testRunner.And("I update the widget warning parameter", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table36 = new TechTalk.SpecFlow.Table(new string[] {
                        "Level",
                        "Branches"});
            table36.AddRow(new string[] {
                        "2",
                        "med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay"});
#line 206
 testRunner.Then("the widget warning parameter is updated with id \'2\'", ((string)(null)), table36, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
