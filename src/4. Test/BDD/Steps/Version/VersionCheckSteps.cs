namespace PH.Well.BDD.Steps.API
{
    using System.Net;
    using Common;
    using Framework;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class VersionCheckSteps
    {
        private ApiClient apiClient;
        private string responseString;

        [When(@"I get the API version")]
        public void WhenIGetTheAPIVersion()
        {
            var endpoint = Configuration.WellApiUrl + "/version";
            apiClient = new ApiClient();
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
            Assert.That(responseString.Contains("version"));
        }
    }
}
