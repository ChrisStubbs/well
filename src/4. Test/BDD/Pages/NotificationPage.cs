namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class NotificationPage : Page
    {
        
          public NotificationPage()
        {
            this.NotificationGrid = new Grid<NotificationGrid> { Locator = By.Id("tableNotifications"), RowLocator = By.ClassName("grid-row") };
            this.Pager = new PagerControl();

        }
        
        protected override string UrlSuffix => "notifications";
        public PagerControl Pager { get; set; }
        public Grid<NotificationGrid> NotificationGrid { get; set; }
    }

    public enum NotificationGrid
    {
        Account = 0,
        PicklistReference = 1,
        InvoiceNumber = 2,
        Contact = 3
    }
}
