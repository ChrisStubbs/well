namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class ApprovalsPage : Page
    {
        public ApprovalsPage()
        {
            this.ApprovalsGrid = new Grid<ApprovalDeliveriesGrid>
            {
                Locator = By.Id("tableApprovalDeliveries"),
                RowLocator = By.ClassName("grid-row")
            };

            this.AccountModal = new AccountModalComponent();
            this.Pager = new PagerControl();
            this.ThresholdRadioGroup = new RadioGroup("thresholdToggle");
            this.ReadOnlyAssigned = new SpanElement() { Locator = By.ClassName("read-only-assigned") };
            this.EnabledAction = new Button() { Locator = By.ClassName("enabled-action") };
            this.DisabledAction = new Button() { Locator = By.ClassName("disabled-action") };
            this.AssignedLink = new Button() { Locator = By.ClassName("assign") };
            this.CurrentUserName = new SpanElement() { Locator = By.Id("current-user-name") };
            this.AssignModal = new AssignModal(Driver);
            OrderByButton = new Image { Locator = By.Id("img-orderByArrow") };
        }

        protected override string UrlSuffix => "approvals";

        public Grid<ApprovalDeliveriesGrid> ApprovalsGrid { get; set; }

        public AccountModalComponent AccountModal { get; set; }

        public PagerControl Pager { get; set; }

        public RadioGroup ThresholdRadioGroup { get; set; }

        public SpanElement ReadOnlyAssigned { get; set; }

        public Button EnabledAction { get; set; }

        public Button DisabledAction { get; set; }

        public Button AssignedLink { get; set; }

        public AssignModal AssignModal { get; set; }

        public SpanElement CurrentUserName { get; set; }

        public Image OrderByButton { get; set; }

        public IWebElement GetLoggedInAssignUserFromModal()
        {
            return AssignModal.GetUserFromModal(CurrentUserName.Text);
        }

        public enum ApprovalDeliveriesGrid
        {
            Route = 0,
            Branch = 1,
            Drop = 2,
            InvoiceNo = 3,
            Account = 4,
            AccountName = 5,
            Cod = 6,
            CreditValue = 7,
            InfoButton = 8,
            Status = 9,
            Threshold = 10,
            Assigned = 11,
            Action = 12,
            DeliveryDate = 13
        }
    }
}
