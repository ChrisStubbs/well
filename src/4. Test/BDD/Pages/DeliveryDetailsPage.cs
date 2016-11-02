namespace PH.Well.BDD.Pages
{
    using System.Linq;

    using Framework.WebElements;
    using OpenQA.Selenium;

    public class DeliveryDetailsPage : Page
    {
        private readonly Div _tabSetDiv;

        public DeliveryDetailsPage()
        {
            this.Grid = new Grid<DeliveryDetailsGrid> { Locator = By.Id("tableDelivery"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
            DeliveryTypeSpan = new SpanElement() { Locator = By.Id("delivery-type") };
            SubmitActionButton = new Button() {Locator = By.Id("submit-button")};
            ConfirmModalButton = new Button() {Locator = By.Id("confirm-modal-button")};

        }

        protected override string UrlSuffix => "delivery";

        public Grid<DeliveryDetailsGrid> Grid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
        public SpanElement DeliveryTypeSpan { get; set; }
        public Button SubmitActionButton { get; set; }
        public Button ConfirmModalButton { get; set; }

        public void ClickCleanTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var button = btnElements.Where(x => x.Text == "Clean").FirstOrDefault();
            button.Click();
        }

        public void ClickExceptionsTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var button = btnElements.Where(x => x.Text == "Exceptions").FirstOrDefault();
            button.Click();
        }
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
