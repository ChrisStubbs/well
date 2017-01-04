namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class RoutesPage : Page
    {
        public RoutesPage()
        {
            this.RoutesGrid = new Grid<RoutesGrid> { Locator = By.Id("tableRoutes"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
            this.ExceptionButton = new Button { Locator = By.Id("btn-exceptions") };
            this.CleanButton = new Button { Locator = By.Id("btn-clean") };
            this.OrderByButton = new Button {Locator = By.Id("sort-me") };
        }

        protected override string UrlSuffix => "routes";

        public Grid<RoutesGrid> RoutesGrid { get; set; }

        public FilterControl Filter { get; set; }

        public PagerControl Pager { get; set; }

        public Button ExceptionButton { get; set; }

        public Button CleanButton { get; set; }

        public Button OrderByButton { get; set; }
    }

    public enum RoutesGrid
    {
        Route,
        Branch,
        RouteDate,
        Driver,
        NoOfDrops,
        Exceptions,
        Clean,
        Status,
        LastUpdatedDateTime
    }
}