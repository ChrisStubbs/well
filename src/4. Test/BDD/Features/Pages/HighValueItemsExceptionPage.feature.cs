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
                    "\n\tso that I can determine\twhich deliveries have been unsuccessful", ProgrammingLanguage.CSharp, new string[] {
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
 testRunner.And("I have loaded the Adam high value route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("I have loaded the Adam order data with high value lines", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("I have imported a valid Epod update file named \'ePOD__20170317_HIGHVALUE.xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("View exception details at lower level with delivery check icon displayed")]
        public virtual void ViewExceptionDetailsAtLowerLevelWithDeliveryCheckIconDisplayed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("View exception details at lower level with delivery check icon displayed", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 30
 testRunner.Given("I have selected branch \'55\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.When("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.And("I click on exception row 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "LineNo",
                        "Product",
                        "Description",
                        "Value",
                        "InvoiceQuantity",
                        "DeliveryQuantity",
                        "DamagedQuantity",
                        "ShortQuantity",
                        "Checked"});
            table1.AddRow(new string[] {
                        "1",
                        "69603",
                        "Carlsberg Exp 4.8% 4Pk",
                        "23.33",
                        "1",
                        "0",
                        "0",
                        "1",
                        "true"});
#line 33
 testRunner.Then("I am shown the exception detail", ((string)(null)), table1, "Then ");
#line 36
 testRunner.And("I am shown the high value check", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
