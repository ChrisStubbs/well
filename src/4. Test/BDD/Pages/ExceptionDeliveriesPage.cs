namespace PH.Well.BDD.Pages
{
    using System;
    using System.Linq;

    using Framework.WebElements;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using PH.Well.BDD.Framework;
    using PH.Well.BDD.Framework.Extensions;

    public class ExceptionDeliveriesPage : Page
    {
        public ExceptionDeliveriesPage()
        {
            this.ExceptionsGrid = new Grid<ExceptionDeliveriesGrid>
            {
                Locator = By.Id("tableExceptionDeliveries"),
                RowLocator = By.ClassName("grid-row")
            };
            this.ExceptionsDrillDownGrid = new Grid<ExceptionDrilldownGrid>
            {
                Locator = By.Id("tableDelivery"),
                RowLocator = By.ClassName("grid-row")
            };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
            this.EnabledButton = new Button { Locator = By.ClassName("enabled-action") };
            this.DeliveryUpdateDrillDown = new Heading { Locator = By.Id("delivery-update") };
            this.OrderByButton = new Image { Locator = By.Id("img-orderByArrow") };
            this.NoExceptionsDiv = new Div { Locator = By.Id("no-exceptions") };
         
            this.CurrentUserName = new SpanElement() { Locator = By.Id("current-user-name") };
            this.BulkCreditButton = new Button { Locator = By.Id("bulk-credit") };
            this.SelectAllButton = new Button { Locator = By.Id("selectAll") };
            this.ModalConfirmButton = new Button { Locator = By.Id("confirm-modal-button") };
            this.ProductInformation = new SpanElement { Locator = By.Id("product-information") };
            this.ShortQty = new SpanElement { Locator = By.Id("short-qty") };
            this.ShortReason = new SpanElement { Locator = By.Id("short-reason") };
            this.ShortSource = new SpanElement { Locator = By.Id("short-source") };
            this.ShortAction = new SpanElement { Locator = By.Id("short-action") };
            this.DamageQty = new SpanElement { Locator = By.Id("damage-qty0") };
            this.DamageReason = new SpanElement { Locator = By.Id("damage-reason0") };
            this.DamageSource = new SpanElement { Locator = By.Id("damage-source0") };
            this.DamageAction = new SpanElement { Locator = By.Id("damage-action0") };

            this.AccountModal = new AccountModalComponent();
            this.AssignModal = new AssignModal(Driver);
            this.BulkModalComponent = new BulkModalComponent();
            ConfirmModal = new ConfirmModal();

            FirstRowSubmitButton = new Button() {Locator = By.Id("submit1")};
        }

        public SpanElement ShortReason { get; set; }

        public SpanElement ShortSource { get; set; }

        public SpanElement ShortAction { get; set; }

        public SpanElement DamageQty { get; set; }

        public SpanElement DamageReason { get; set; }

        public SpanElement DamageSource { get; set; }

        public SpanElement DamageAction { get; set; }

        protected override string UrlSuffix => "exceptions";

        public Grid<ExceptionDeliveriesGrid> ExceptionsGrid { get; set; }

        public Grid<ExceptionDrilldownGrid> ExceptionsDrillDownGrid { get; set; }

        public FilterControl Filter { get; set; }

        public PagerControl Pager { get; set; }

        public Button EnabledButton { get; set; }

        public BulkModalComponent BulkModalComponent { get; set; }

        public ConfirmModal ConfirmModal { get; set; }

        
        public CheckBox CreditCheckBox(int lineIdx)
        {
          return  new CheckBox { Locator = By.Id($"checkbox-credit-{lineIdx}") };
        }

        public Button BulkCreditButton { get; set; }

        public Button SelectAllButton { get; set; }

        public Image OrderByButton { get; set; }

        public Heading DeliveryUpdateDrillDown { get; set; }

        public AccountModalComponent AccountModal { get; set; }

        public readonly Button ModalConfirmButton;
    
        public Div NoExceptionsDiv { get; set; }

        public SpanElement CurrentUserName { get; set; } 

        public AssignModal AssignModal { get; set; }

        public SpanElement ProductInformation { get; set; }

        public SpanElement ShortQty { get; set; }

        public Button FirstRowSubmitButton { get; set; }

        public IWebElement GetFirstCell()
        {
            this.Driver.WaitForJavascript();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName("first-cell")));

            return elements.First();
        }

        public IWebElement GetLoggedInAssignUserFromModal()
        {
            return AssignModal.GetUserFromModal(CurrentUserName.Text);
        }

        public int GetCountOfElements(string className)
        {
            this.Driver.WaitForJavascript();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName(className)));

            return elements.Count();
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

    public enum ExceptionDeliveriesGrid
    {
        Route = 0,
        Drop = 1,
        InvoiceNo = 2,
        Account = 3,
        AccountName= 4,
        Cod = 5,
        CreditValue = 6,
        InfoButton = 7,
        Status = 8,
        ToBeAdvised = 11,
        Reason = 12,
        Assigned = 13,
        Action = 14,
        LastUpdatedDateTime = 15
    }

    public enum ExceptionDrilldownGrid
    {
        LineNumber = 0,
        Product = 1,
        Description = 2,
        Value = 3,
        InvoiceQuantity = 4,
        DeliveredQuantity = 5,
        DamagedQuantity = 6,
        ShortQuantity = 7,
        Update = 8
    }
}
