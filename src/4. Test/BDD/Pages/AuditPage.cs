namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class AuditPage : Page
    {
        public AuditPage()
        {
            this.Grid = new Grid<AuditGrid> { Locator = By.Id("tableAudits"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
        }

        protected override string UrlSuffix => "audits";

        public Grid<AuditGrid> Grid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
    }

    public enum AuditGrid
    {
        Entry = 0,
        Type = 1,
        InvoiceNo = 2,
        Account = 3,
        DeliveryDate = 4
    }
}
