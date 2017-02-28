namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class WidgetsPage : Page
    {
        public WidgetsPage()
        {
            this.UnsubmittedExceptionsSpan = new SpanElement { Locator = By.ClassName("unsubmitted-exceptions") };
            this.UnapprovedExceptionsSpan = new SpanElement { Locator = By.ClassName("approval-exceptions") };
            this.UnsubmittedAssignedSpan = new SpanElement { Locator = By.ClassName("my-unsubmitted-exceptions") };
            this.UnapprovedAssignedSpan = new SpanElement { Locator = By.ClassName("my-approval-exceptions") };
            this.UnsubmittedOutstandingSpan = new SpanElement { Locator = By.ClassName("outstanding-unsubmitted-exceptions") };
            this.UnapprovedOutstandingSpan = new SpanElement { Locator = By.ClassName("outstanding-approval-exceptions") };
            this.NotificationsSpan = new SpanElement { Locator = By.ClassName("notifications") };
        }

        protected override string UrlSuffix => "widgets";

        public SpanElement UnsubmittedExceptionsSpan { get; set; }
        public SpanElement UnapprovedExceptionsSpan { get; set; }
        public SpanElement UnsubmittedAssignedSpan { get; set; }
        public SpanElement UnapprovedAssignedSpan { get; set; }
        public SpanElement UnsubmittedOutstandingSpan { get; set; }
        public SpanElement UnapprovedOutstandingSpan { get; set; }
        public SpanElement NotificationsSpan { get; set; }


    }

}
