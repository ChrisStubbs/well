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
    [NUnit.Framework.DescriptionAttribute("Edit delivery line")]
    [NUnit.Framework.CategoryAttribute("WebDriverFeature")]
    [NUnit.Framework.CategoryAttribute("RoleSuperUser")]
    public partial class EditDeliveryLineFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "EditDeliveryLine.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Edit delivery line", "\tAs a customer service agent\r\n\tI wish to be able to edit short qtys and damages o" +
                    "n deliveries \r\n\tso that I can fix any discrepancies raised by customers", ProgrammingLanguage.CSharp, new string[] {
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
 testRunner.And("I have selected branch \'22\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add short qty and damages to clean delivery")]
        public virtual void AddShortQtyAndDamagesToCleanDelivery()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add short qty and damages to clean delivery", ((string[])(null)));
#line 13
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 14
 testRunner.Given("1 deliveries have been marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
 testRunner.And("there are no proof of deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 16
 testRunner.And("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.And("I assign the clean delivery to myself", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.And("I open the clean delivery \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 19
 testRunner.And("I click on the first delivery line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.When("I enter a short quantity of \'5\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.And("I select a short source of \'Checker\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
 testRunner.And("I select a short reason of \'Minimum Drop Charge\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.And("I select a short action of \'Credit\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
 testRunner.And("click add damage button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
 testRunner.And("I enter a damage qty of \'2\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
 testRunner.And("I enter a damage reason of \'Picking Error\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.And("I enter a damage source of \'Customer\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
 testRunner.And("I enter a damage action of \'Credit\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
 testRunner.And("I save the delivery line updates", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
 testRunner.And("I confirm the delivery line update", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
 testRunner.Then("the line \'1\' Short Qty is \'5\' and Damaged Qty is \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 32
 testRunner.And("the delivery status is \'Exception\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
 testRunner.When("I open the audits page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Entry",
                        "Type",
                        "InvoiceNo",
                        "Account",
                        "DeliveryDate"});
            table1.AddRow(new string[] {
                        "Product: 50035 - Ind Potato Gratin 400g. Short Qty changed from 0 to 5. Damages a" +
                            "dded Reason - Picking Error, Source - Customer, Action - Credit - 2.",
                        "DeliveryLineUpdate",
                        "94294343",
                        "49214.152 - CSG - must be CF van",
                        "07/01/2016"});
#line 34
 testRunner.Then("the following audit entries are shown", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Remove short qty and damages from exception delivery")]
        public virtual void RemoveShortQtyAndDamagesFromExceptionDelivery()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Remove short qty and damages from exception delivery", ((string[])(null)));
#line 38
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 39
 testRunner.Given("1 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 40
 testRunner.And("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 41
 testRunner.And("I assign the delivery to myself", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
 testRunner.And("I open the exception delivery \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
 testRunner.And("I click on the first delivery line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.When("I enter a short quantity of \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 45
 testRunner.And("I save the delivery line updates", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 46
 testRunner.And("I confirm the delivery line update", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
 testRunner.And("I open the clean tab", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
 testRunner.Then("the line \'1\' Short Qty is \'0\' and Damaged Qty is \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 49
 testRunner.And("the delivery status is \'Resolved\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.When("I open the audits page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Entry",
                        "Type",
                        "InvoiceNo",
                        "Account",
                        "DeliveryDate"});
            table2.AddRow(new string[] {
                        "Product: 50035 - Ind Potato Gratin 400g. Short Qty changed from 1 to 0.",
                        "DeliveryLineUpdate",
                        "976549",
                        "49214.152 - CSG - must be CF van",
                        "07/01/2016"});
#line 51
 testRunner.Then("the following audit entries are shown", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can not edit unassigned delivery line")]
        public virtual void CanNotEditUnassignedDeliveryLine()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can not edit unassigned delivery line", ((string[])(null)));
#line 55
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 56
 testRunner.Given("I have imported a valid Epod update file named \'ePOD_30062016_Update.xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 57
 testRunner.And("1 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.When("I view the Issues for line \'1\' of Delivery \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 59
 testRunner.Then("I cannot add or edit any shorts or damages", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Can not edit delivery line assigned to another user")]
        public virtual void CanNotEditDeliveryLineAssignedToAnotherUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Can not edit delivery line assigned to another user", ((string[])(null)));
#line 61
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 62
 testRunner.Given("I have imported a valid Epod update file named \'ePOD_30062016_Update.xml\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 63
 testRunner.And("1 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.And("the exception is assigned to identity: \'palmerharvey\\Bruno.Dobson\', name: \'Bruno " +
                    "Dobson\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 65
 testRunner.When("I view the Issues for line \'1\' of Delivery \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 66
 testRunner.Then("I cannot add or edit any shorts or damages", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add short qty and damages to exception delivery")]
        public virtual void AddShortQtyAndDamagesToExceptionDelivery()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add short qty and damages to exception delivery", ((string[])(null)));
#line 68
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 69
 testRunner.Given("1 deliveries have been marked as exceptions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 70
 testRunner.And("there are no proof of deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 71
 testRunner.And("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 72
 testRunner.And("I assign the delivery to myself", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 73
 testRunner.And("I open the exception delivery \'1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.And("I click on the first delivery line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.When("I enter a short quantity of \'5\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 76
 testRunner.And("I select a short source of \'Checker\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
 testRunner.And("I select a short reason of \'Minimum Drop Charge\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
 testRunner.And("I select a short action of \'Credit\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
 testRunner.And("click add damage button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 80
 testRunner.And("I enter a damage qty of \'2\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
 testRunner.And("I enter a damage reason of \'Picking Error\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 82
 testRunner.And("I enter a damage source of \'Customer\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
 testRunner.And("I enter a damage action of \'Credit\' for id \'0\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 84
 testRunner.And("I save the delivery line updates", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
 testRunner.And("I open the exception deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.And("I select the exception submit button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 87
 testRunner.Then("I can see the product information \'50035 Ind Potato Gratin 400g\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 88
 testRunner.And("I can see the shortage quantity of \'5\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
 testRunner.And("I can see the shortage reason of \'Minimum Drop Charge\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
 testRunner.And("I can see the shortage source of \'Checker\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
 testRunner.And("I can see the shortage action of \'Credit\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
 testRunner.And("I can see the damage quantity of \'2\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
 testRunner.And("I can see the damage reason of \'Picking Error\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
 testRunner.And("I can see the damage source of \'Customer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
 testRunner.And("I can see the damage action of \'Credit\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Crediting is disabled for deliveries with POD (Proof of delivery)")]
        public virtual void CreditingIsDisabledForDeliveriesWithPODProofOfDelivery()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Crediting is disabled for deliveries with POD (Proof of delivery)", ((string[])(null)));
#line 97
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 98
 testRunner.Given("All the deliveries are marked as clean", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 99
 testRunner.And("I open the clean deliveries", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
 testRunner.And("I assign the POD delivery to myself", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
 testRunner.And("I click on the first POD delivery", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
 testRunner.And("I open the clean tab", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
 testRunner.When("I click on the first delivery line", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Action"});
            table3.AddRow(new string[] {
                        "Not Defined"});
            table3.AddRow(new string[] {
                        "Close"});
#line 104
 testRunner.Then("the available short actions are", ((string)(null)), table3, "Then ");
#line 108
 testRunner.When("click add damage button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Action"});
            table4.AddRow(new string[] {
                        "Not Defined"});
            table4.AddRow(new string[] {
                        "Close"});
#line 109
 testRunner.Then("the available damage actions are", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
