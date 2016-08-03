namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class DeliveryDetailsPage : Page
    {
        public DeliveryDetailsPage()
        {
            this.Grid = new Grid<DeliveryDetailsGrid> { Locator = By.Id("tableDelivery"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
        }

        protected override string UrlSuffix => "exceptions";

        public Grid<DeliveryDetailsGrid> Grid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
    }

    public enum DeliveryDetailsGrid
    {
        LineNo = 0,
        Product = 1,
        Description = 2,
        Value = 3,
        InvoiceQuantity = 4,
        DeliveryQuantity = 5,
        DamagedQuantity = 6,
        ShortQuantity = 7
    }
}
