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
    [NUnit.Framework.DescriptionAttribute("HighValueItemsExceptionPage")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
    public partial class HighValueItemsExceptionPageFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "HighValueItemsExceptionPage.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "HighValueItemsExceptionPage", "\tAs a user\r\n\tI wish to be able to view and filter exception delivery information\r" +
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
        [NUnit.Framework.DescriptionAttribute("A user can view Exception Delivery Information with shorts to be advised displaye" +
            "d")]
        public virtual void AUserCanViewExceptionDeliveryInformationWithShortsToBeAdvisedDisplayed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A user can view Exception Delivery Information with shorts to be advised displaye" +
                    "d", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 15
 testRunner.Given("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 16
 testRunner.And("2 deliveries have been marked as exceptions with shorts to be advised", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Route",
                        "Drop",
                        "InvoiceNo",
                        "Account",
                        "AccountName",
                        "Status",
                        "TBA"});
            table1.AddRow(new string[] {
                        "001",
                        "1",
                        "94294343",
                        "49214.152",
                        "CSG - must be CF van",
                        "Incomplete",
                        "2"});
            table1.AddRow(new string[] {
                        "001",
                        "1",
                        "92545470",
                        "2874.033",
                        "CSG - must be CF van",
                        "Incomplete",
                        "2"});
#line 18
 testRunner.Then("the following exception deliveries will be displayed", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("View exception details at lower level with delivery check icon displayed")]
        public virtual void ViewExceptionDetailsAtLowerLevelWithDeliveryCheckIconDisplayed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View exception details at lower level with delivery check icon displayed", ((string[])(null)));
#line 23
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 24
 testRunner.Given("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 25
 testRunner.And("All the deliveries are marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
 testRunner.And("All delivery lines are flagged with line delivery status \'Exception\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.And("I click on exception row 4", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo",
                        "Product",
                        "Description",
                        "Value",
                        "InvoiceQuantity",
                        "DeliveryQuantity",
                        "DamagedQuantity",
                        "ShortQuantity"});
            table2.AddRow(new string[] {
                        "1",
                        "6987",
                        "Choc Teacakes Tunnock",
                        "19.23",
                        "1",
                        "-1",
                        "0",
                        "2"});
            table2.AddRow(new string[] {
                        "2",
                        "49179",
                        "Ginger Nuts 250g",
                        "4.88",
                        "1",
                        "-1",
                        "0",
                        "2"});
            table2.AddRow(new string[] {
                        "3",
                        "21633",
                        "Kiddies Super Mix 220gPM",
                        "3.60",
                        "1",
                        "-1",
                        "0",
                        "2"});
            table2.AddRow(new string[] {
                        "4",
                        "4244",
                        "Milkybar Btns Giant PM",
                        "5.60",
                        "1",
                        "-1",
                        "0",
                        "2"});
            table2.AddRow(new string[] {
                        "5",
                        "7621",
                        "Fruit Past Tube 52.5g",
                        "8.40",
                        "1",
                        "-1",
                        "0",
                        "2"});
#line 29
 testRunner.Then("I am shown the exception detail", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
