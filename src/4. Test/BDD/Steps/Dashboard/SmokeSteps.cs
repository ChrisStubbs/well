namespace PH.Well.BDD.Steps.Dashboard
{
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class SmokeSteps
    {
        [When(@"I view the homepage")]
        public void WhenIViewTheHomepage()
        {
            HomePage.Open();
        }
        
        [Then(@"I can see the message ""(.*)""")]
        public void ThenICanSeeTheMessage(string p0)
        {
            Assert.AreEqual(p0, HomePage.H1Heading.Content);
        }

        private HomePage HomePage
        {
            get
            {
                return new HomePage();
            }
        }
    }
}
