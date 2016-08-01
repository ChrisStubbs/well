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
namespace PH.Well.BDD.Features.ACL
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("EpodRouteFileSchemaValidation")]
    public partial class EpodRouteFileSchemaValidationFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "EpodRouteFileSchemaValidation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "EpodRouteFileSchemaValidation", "\tIn order to import correctly formed Epod route files\r\n\tAs a math idiot\r\n\tI want " +
                    "to be able to validate exsisting Epod route files against a pre defined schema", ProgrammingLanguage.CSharp, ((string[])(null)));
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
  testRunner.Given("I have loaded the Adam route data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Import Epod route file with the JobDetailID child node missing from the first Job" +
            "DetailDamage node")]
        public virtual void ImportEpodRouteFileWithTheJobDetailIDChildNodeMissingFromTheFirstJobDetailDamageNode()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Import Epod route file with the JobDetailID child node missing from the first Job" +
                    "DetailDamage node", ((string[])(null)));
#line 10
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 11
 testRunner.Given("I have an invalid Epod route file \'ePOD_MissingJobDetailIdNode.xml\' with a \'JobDe" +
                    "tailDamage\' node at position \'0\' with the \'JobDetailID\' node missing", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 12
 testRunner.When("I import the route file \'ePOD_MissingJobDetailIdNode.xml\' into the well", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 13
 testRunner.Then(@"The schema validation error should be ""file ePOD_MissingJobDetailIdNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'JobDetailDamage' has invalid child element 'DamageReasonID'. List of possible elements expected: 'JobDetailID'.""", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Import Epod route file with the Qty child node missing from the first JobDetailDa" +
            "mage node")]
        public virtual void ImportEpodRouteFileWithTheQtyChildNodeMissingFromTheFirstJobDetailDamageNode()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Import Epod route file with the Qty child node missing from the first JobDetailDa" +
                    "mage node", ((string[])(null)));
#line 15
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 16
 testRunner.Given("I have an invalid Epod route file \'ePOD_MissingQtyNode.xml\' with a \'JobDetailDama" +
                    "ge\' node at position \'0\' with the \'Qty\' node missing", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 17
 testRunner.When("I import the route file \'ePOD_MissingQtyNode.xml\' into the well", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 18
 testRunner.Then("The schema validation error should be \"file ePOD_MissingQtyNode.xml failed schema" +
                    " validation with the following: System.Xml.XsdValidatingReader:\tThe element \'Job" +
                    "DetailDamage\' has invalid child element \'Deleted\'. List of possible elements exp" +
                    "ected: \'Qty\'.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Import Epod route file with the ReasonCode child node missing from the first Reas" +
            "on node")]
        public virtual void ImportEpodRouteFileWithTheReasonCodeChildNodeMissingFromTheFirstReasonNode()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Import Epod route file with the ReasonCode child node missing from the first Reas" +
                    "on node", ((string[])(null)));
#line 20
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 21
 testRunner.Given("I have an invalid Epod route file \'ePOD_MissingReasonCodeNode.xml\' with a \'Reason" +
                    "\' node at position \'0\' with the \'ReasonCode\' node missing", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 22
 testRunner.When("I import the route file \'ePOD_MissingReasonCodeNode.xml\' into the well", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 23
 testRunner.Then(@"The schema validation error should be ""file ePOD_MissingReasonCodeNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Reason' has invalid child element 'Description'. List of possible elements expected: 'ReasonCode'.""", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Import Epod route file with a duplicate ReasonCode node added to the first first " +
            "JobDamageDetail node")]
        public virtual void ImportEpodRouteFileWithADuplicateReasonCodeNodeAddedToTheFirstFirstJobDamageDetailNode()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Import Epod route file with a duplicate ReasonCode node added to the first first " +
                    "JobDamageDetail node", ((string[])(null)));
#line 25
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 26
 testRunner.Given("I have an invalid Epod route file \'ePOD__AdditionalDamageReasonCode.xml\' with a \'" +
                    "Reason\' node at position \'0\' with a \'ReasonCode\' node added with a value of \'CAR" +
                    "01\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 27
 testRunner.When("I import the route file \'ePOD__AdditionalDamageReasonCode.xml\' into the well", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.Then("The schema validation error should be \"file ePOD__AdditionalDamageReasonCode.xml " +
                    "failed schema validation with the following: System.Xml.XsdValidatingReader:\tThe" +
                    " element \'Reason\' has invalid child element \'ReasonCode\'.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
