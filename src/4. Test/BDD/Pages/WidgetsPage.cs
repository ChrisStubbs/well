namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class WidgetsPage : Page
    {
        public WidgetsPage()
        {
            this.ExceptionsSpan = new SpanElement { Locator = By.ClassName("Exceptions-count") };
            this.AssignedSpan = new SpanElement { Locator = By.ClassName("Assigned-count") };
            this.OutstandingSpan = new SpanElement { Locator = By.ClassName("Outstanding-count") };
            this.NotificationsSpan = new SpanElement { Locator = By.ClassName("Notifications-count") };
        }

        protected override string UrlSuffix => "widgets";

        public SpanElement ExceptionsSpan { get; set; }
        public SpanElement AssignedSpan { get; set; }
        public SpanElement OutstandingSpan { get; set; }
        public SpanElement NotificationsSpan { get; set; }


    }

}
