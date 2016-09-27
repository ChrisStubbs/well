namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class NotificationPageSteps
    {
        private NotificationPage NotificationPage => new NotificationPage();

        [When(@"I navigate to the notifications page")]
        public void WhenINavigateToTheNotificationsPage()
        {
            NotificationPage.Open();
        }

        [Then(@"I will have (.*) pages of notification data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(NotificationPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

        [Then(@"'(.*)' rows of notification data will be displayed")]
        public void ThenRowsOfNotificationDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.NotificationPage.NotificationGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }
    }
}
