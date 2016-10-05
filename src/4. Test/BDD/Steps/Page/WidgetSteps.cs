namespace PH.Well.BDD.Steps.Page
{
    using NUnit.Framework;
    using PH.Well.BDD.Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class WidgetSteps
    {
        private WidgetsPage widgetsPage => new WidgetsPage();

        [When("I view the widgets page")]
        public void NavigateToUserPreferences()
        {
            widgetsPage.Open();
        }

        [Then(@"there are (.*) exceptions, (.*) assigned, (.*) outstanding and (.*) notifications")]
        public void ThenThereAreExceptionsAssignedOutstandingAndNotifications(string exceptions, string assigned,
            string outstanding, string notifications)
        {
            Assert.AreEqual(exceptions, widgetsPage.ExceptionsSpan.Text);
            Assert.AreEqual(assigned, widgetsPage.AssignedSpan.Text);
            Assert.AreEqual(outstanding, widgetsPage.OutstandingSpan.Text);
            Assert.AreEqual(notifications, widgetsPage.NotificationsSpan.Text);
        }
    }
}