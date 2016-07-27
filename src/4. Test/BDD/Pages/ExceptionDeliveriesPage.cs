namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class ExceptionDeliveriesPage : Page
    {
        public ExceptionDeliveriesPage()
        {
            this.RoutesGrid = new Grid<ExceptionDeliveriesGrid> { Locator = By.Id("tableExceptionDeliveries"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
        }
        protected override string UrlSuffix => "Exceptions";

        public Grid<ExceptionDeliveriesGrid> RoutesGrid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
    }

    public enum ExceptionDeliveriesGrid
    {
        Route = 0,
        Drop = 1,
        InvoiceNo = 2,
        Account = 3,
        AccountName= 4,
        Contact= 5,
        InfoButton = 6,
        Status = 7,
        Reason = 8,
        Assigned = 9,
        Action = 10,
        LastUpdatedDateTime = 11
    }
}
