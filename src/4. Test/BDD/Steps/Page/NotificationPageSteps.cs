namespace PH.Well.BDD.Steps.Page
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Framework.WebElements;
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

        [Then(@"'(.*)' rows of notification data will be displayed on page (.*)")]
        public void ThenRowsOfNotificationDataWillBeDisplayed(int noOfRowsExpected, int pageNo)
        {
            var notifications = this.NotificationPage.GetNotifications(noOfRowsExpected, pageNo);

            Assert.That(notifications.Count, Is.EqualTo(noOfRowsExpected));
        }

        [When(@"I click on notification page (.*)")]
        public void WhenIClickOnNotificationPage(int pageNo)
        {
            this.NotificationPage.Pager.Click(pageNo);
        }

        [Then(@"the following notifications with a rowcount of '(.*)' will be displayed on page (.*)")]
        public void ThenTheFollowingNotificationsWithARowcountOfWillBeDisplayed(int rowCount, int pageNo, Table table)
        {
            var notifications = this.NotificationPage.GetNotifications(rowCount, pageNo);

            Assert.That(notifications.Count, Is.EqualTo(table.RowCount));

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(notifications[i].Account.Text, Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(notifications[i].Invoice.Text, Is.EqualTo(table.Rows[i]["InvoiceNumber"]));
                Assert.That(notifications[i].ErrorNumber.Text, Is.EqualTo(table.Rows[i]["AdamErrorNumber"]));
                Assert.That(notifications[i].ErrorMessage.Text, Is.EqualTo(table.Rows[i]["ErrorMessage"]));
                Assert.That(notifications[i].CrossReference.Text, Is.EqualTo(table.Rows[i]["CrossReference"]));
                Assert.That(notifications[i].ErrorMessage.Text, Is.EqualTo(table.Rows[i]["ErrorMessage"]));
                Assert.That(notifications[i].User.Text, Is.EqualTo(table.Rows[i]["UserName"]));
            }
        }


        [Then(@"the following notification will not be displayed on page (.*)")]
        public void ThenTheFollowingNotificationsWillNotBeDisplayedOnPage(int rowCount, int pageNo, Table table)
        {
            var notifications = this.NotificationPage.GetNotifications(rowCount, pageNo);

            Assert.That(notifications.Count, Is.EqualTo(table.RowCount));

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(notifications[i].Account.Text, Is.EqualTo(table.Rows[i]["Account"]));
               //Assert.That(notifications[i].Pick.Text, Is.EqualTo(table.Rows[i]["PicklistReference"]));
                Assert.That(notifications[i].Invoice.Text, Is.EqualTo(table.Rows[i]["InvoiceNumber"]));
                //Assert.That(notifications[i].Contact.Text, Is.EqualTo(table.Rows[i]["Contact"]));
                //Assert.That(notifications[i].Reason.Text, Is.EqualTo(table.Rows[i]["Reason"]));
            }
        }


        [When(@"I archive the notification (.*) from rowcount (.*) on page (.*)")]
        public void ArchiveTheNotification(int row, int rowCount, int pageNo)
        {
            var notifications = this.NotificationPage.GetNotifications(rowCount, pageNo);
            notifications[row - 1].Archive.Click();

        }

        [Then(@"I can see the following notification detail")]
        public void ThenICanSeeTheFollowingNotificationDetail(Table table)
        {
            var modal = NotificationPage.ArchiveModal;
            ArchiveModalSteps.CompareModal(table, modal);
        }


        [When(@"I click 'Yes' on the archive modal")]
        public void ClickYesOnTheArchiveModal()
        {
            var modal = NotificationPage.ArchiveModal;
            ArchiveModalSteps.ClickYes(modal);
        }

    }
}
