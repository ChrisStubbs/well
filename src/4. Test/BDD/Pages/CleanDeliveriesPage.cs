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

        public IWebElement GetLoggedInAssignUserFromModal()
        {
            return AssignModal.GetUserFromModal(CurrentUserName.Text);
        }
    }

    public enum CleanDeliveriesGrid
    {
        Route = 0,
        Drop = 1,
        InvoiceNo = 2,
        Account = 3,
        AccountName= 4,
        Contact= 5,
        Assigned = 6,
        LastUpdatedDateTime = 7
    }
}
