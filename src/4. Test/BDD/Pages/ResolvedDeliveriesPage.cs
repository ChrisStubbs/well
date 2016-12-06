namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class ResolvedDeliveriesPage : Page
    {
        public ResolvedDeliveriesPage()
        {
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
            this.OrderByButton = new Image { Locator = By.Id("img-orderByArrow") };
            AccountModal = new AccountModalComponent();
        }
        protected override string UrlSuffix => "resolved";

        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
        public Image OrderByButton { get; set; }
        public AccountModalComponent AccountModal { get; set; }

        public ResolvedGridRow GetGridRow(int id)
        {
            return new ResolvedGridRow(id);
        }
    }

    public class ResolvedGridRow
    {
        public const string RouteId = "resolved-route-";
        public const string DropId = "resolved-drop-";
        public const string InvoiceId = "resolved-invoice-";
        public const string CodeId = "resolved-code-";
        public const string NameId = "resolved-name-";
        public const string CodId = "resolved-cod-";
        public const string JobId = "resolved-job-";
        public const string AssignedId = "resolved-assigned-";
        public const string DateId = "resolved-date-";
        public const string ContactId = "resolved-contact-";

        public ResolvedGridRow(int id)
        {
            this.Route = new SpanElement { Locator = By.Id(RouteId + id) };
            this.Drop = new SpanElement { Locator = By.Id(DropId + id) };
            this.Invoice = new SpanElement { Locator = By.Id(InvoiceId + id) };
            this.Code = new SpanElement { Locator = By.Id(CodeId + id) };
            this.Name = new SpanElement { Locator = By.Id(NameId + id) };
            this.Cod = new SpanElement { Locator = By.Id(CodId + id) };
            this.Job = new SpanElement { Locator = By.Id(JobId + id) };
            this.Assigned = new SpanElement { Locator = By.Id(AssignedId + id) };
            this.Date = new SpanElement { Locator = By.Id(DateId + id) };
            this.Contact = new Button { Locator = By.Id(ContactId + id) };
        }
        
        public SpanElement Route { get; set; }

        public SpanElement Drop { get; set; }

        public SpanElement Invoice { get; set; }

        public SpanElement Code { get; set; }

        public SpanElement Name { get; set; }

        public SpanElement Cod { get; set; }

        public SpanElement Job { get; set; }

        public SpanElement Assigned { get; set; }

        public SpanElement Date { get; set; }

        public Button Contact { get; set; }
    }
}
