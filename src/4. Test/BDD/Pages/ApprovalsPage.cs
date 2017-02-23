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
            ThresholdRadioGroup = new RadioGroup("thresholdToggle");
        }

        protected override string UrlSuffix => "approvals";

        public Grid<ApprovalDeliveriesGrid> ApprovalsGrid { get; set; }

        public AccountModalComponent AccountModal { get; set; }

        public PagerControl Pager { get; set; }

        public RadioGroup ThresholdRadioGroup { get; set; }

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
