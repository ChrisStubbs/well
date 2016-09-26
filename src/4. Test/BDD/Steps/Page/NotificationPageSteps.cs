namespace PH.Well.BDD.Steps.Page
{
    using TechTalk.SpecFlow;
    using PH.Well.BDD.Pages;



    [Binding]
    public class NotificationPageSteps
    {
        private NotificationPage NotificationPage => new NotificationPage();

        [When(@"I navigate to the notifications page")]
        public void WhenINavigateToTheNotificationsPage()
        {
            NotificationPage.Open();
        }
    }
}
