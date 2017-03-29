namespace PH.Well.BDD.Pages
{
    using System.Collections.Generic;
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
            JobStatusSpan = new SpanElement() { Locator = By.Id("job-status") };
            SubmitActionButton = new Button() {Locator = By.Id("submit-button")};
            ConfirmModalButton = new Button() {Locator = By.Id("confirm-modal-button")};
            NoExceptions = new Div() { Locator = By.Id("no-exceptions") };

        }

        protected override string UrlSuffix => "delivery";

        public Grid<DeliveryDetailsGrid> Grid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
        public SpanElement JobStatusSpan { get; set; }
        public Button SubmitActionButton { get; set; }
        public Button ConfirmModalButton { get; set; }
        public Div NoExceptions { get; set; }
        public bool HasThisNumberOfHighvalueItems(int count)
        {
            var highValueItems = this.Driver.FindElements(By.ClassName("high-value"));

            return highValueItems.Count == count;
        }

        public void ClickCleanTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var button = btnElements.FirstOrDefault(x => x.Text == "Clean");
            button.Click();
        }

        public void ClickExceptionsTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var button = btnElements.FirstOrDefault(x => x.Text == "Exceptions");
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
