namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class CleanDeliveriesPage : Page
    {
        public CleanDeliveriesPage()
        {
            this.RoutesGrid = new Grid<CleanDeliveriesGrid> { Locator = By.Id("tableCleanDeliveries"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
        }

        protected override string UrlSuffix => "clean";

        public Grid<CleanDeliveriesGrid> RoutesGrid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
    }

    public enum CleanDeliveriesGrid
    {
        Route = 0,
        Drop = 1,
        InvoiceNo = 2,
        Account = 3,
        AccountName= 4,
        Contact= 5,
        InfoButton = 6,
        //Status = 7,
        LastUpdatedDateTime = 8
    }
}
