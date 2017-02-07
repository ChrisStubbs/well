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
            this.OrderByButton = new Image { Locator = By.Id("img-orderByArrow") };
            CurrentUserName = new SpanElement() { Locator = By.Id("current-user-name") };
            CashOnDelivery = new SpanElement() { Locator = By.Id("isCod") };

            AccountModal = new AccountModalComponent();
            AssignModal = new AssignModal(Driver);
        }

        protected override string UrlSuffix => "clean";

        public Grid<CleanDeliveriesGrid> RoutesGrid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }

        public Image OrderByButton { get; set; }
        public AccountModalComponent AccountModal { get; set; }
        public AssignModal AssignModal { get; set; }
        public SpanElement CurrentUserName { get; set; }

        public SpanElement CashOnDelivery { get; set; }

        public IWebElement GetLoggedInAssignUserFromModal()
        {
            return AssignModal.GetUserFromModal(CurrentUserName.Text);
        }

        public bool IsElementPresent(string elementName)
        {
            try
            {
                this.Driver.FindElement(By.Id(elementName));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

    public enum CleanDeliveriesGrid
    {
        Route = 0,
        Branch = 1,
        Drop = 2,
        InvoiceNo = 3,
        Account = 4,
        AccountName = 5,
        CashOnDelivery = 6,
        Contact= 7,
        Assigned = 8,
        LastUpdatedDateTime = 9
    }
}
