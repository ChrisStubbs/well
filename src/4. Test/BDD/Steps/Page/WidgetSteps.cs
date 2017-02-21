namespace PH.Well.BDD.Steps.Page
{
    using NUnit.Framework;
    using Pages;
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

        [Then(@"there are the following widget stats")]
        public void ThenThereAreTheFollowingWidgetStats(Table table)
        {
            var unsubmittedExceptions = table.Rows[0]["Unsubmitted exceptions"];
            var unapprovedExceptions = table.Rows[0]["Unapproved exceptions"];
            var unsubmittedAssigned = table.Rows[0]["Unsubmitted assigned"];
            var unapprovedAssigned = table.Rows[0]["Unapproved assigned"];
            var unsubmittedOutstanding = table.Rows[0]["Unsubmitted outstanding"];
            var unapprovedOutstanding = table.Rows[0]["Unapproved outstanding"];
            var notifications = table.Rows[0]["Notifications"];

            Assert.AreEqual(unsubmittedExceptions, widgetsPage.UnsubmittedExceptionsSpan.Text);
            Assert.AreEqual(unapprovedExceptions, widgetsPage.UnapprovedExceptionsSpan.Text);
            Assert.AreEqual(unsubmittedAssigned, widgetsPage.UnsubmittedAssignedSpan.Text);
            Assert.AreEqual(unapprovedAssigned, widgetsPage.UnapprovedAssignedSpan.Text);
            Assert.AreEqual(unsubmittedOutstanding, widgetsPage.UnsubmittedOutstandingSpan.Text);
            Assert.AreEqual(unapprovedOutstanding, widgetsPage.UnapprovedOutstandingSpan.Text);
            Assert.AreEqual(notifications, widgetsPage.NotificationsSpan.Text);
        }
    }
}