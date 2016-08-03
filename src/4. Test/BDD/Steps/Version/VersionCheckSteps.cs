namespace PH.Well.BDD.Steps.API
{
    using System.Net;

    using Framework;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    using WebClient = PH.Well.Common.WebClient;

    [Binding]
    public class VersionCheckSteps
    {
        private WebClient apiClient;
        private string responseString;

        [When(@"I get the API version")]
        public void WhenIGetTheAPIVersion()
        {
            var endpoint = Configuration.WellApiUrl + "/version";
            apiClient = new WebClient();
            responseString = apiClient.DownloadString(endpoint);
        }
        
        [Then(@"the response code is '(.*)'.*")]
        public void ThenTheResponseCodeIsOK(int statusCode)
        {
            Assert.AreEqual((HttpStatusCode) statusCode, apiClient.HttpWebResponse.StatusCode);
        }
        
        [Then(@"a version number is returned")]
        public void ThenAVersionNumberIsReturned()
        {
            Assert.That(string.IsNullOrEmpty(responseString) == false);
        }
    }
}
