namespace PH.Well.BDD.Pages
{
    using System.Collections.Generic;
    using System.Linq;
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class WidgetWarningPage : Page
    {
        public readonly TextBox Level;
        public readonly TextBox Description;
        public readonly Button Save;
        public readonly AdminButtonDropDown AdminDropDown;
        public readonly WidgetButtonDropDown WidgetButtonDropDown;
        public readonly Button Remove;
        public readonly Button Add;
        public readonly TextBox WidgetType;
        public readonly SpanElement NoResults;

        public WidgetWarningPage()
        {
            this.Save = new Button { Locator = By.Id("widget-save") };
            this.Add = new Button { Locator = By.Id("add-widget-warning") };
            this.Remove = new Button { Locator = By.Id("widget-warning-remove-confirm") };
            this.AdminDropDown = new AdminButtonDropDown {Locator = By.Id("admin-dropdown")};
            this.WidgetButtonDropDown = new WidgetButtonDropDown {Locator = By.Id("widget-warning-dropdown")};
            this.Level = new TextBox {Locator = By.Id("warning-level") };
            this.Description = new TextBox {Locator = By.Id ("warning-description")};
            this.WidgetType = new TextBox {Locator = By.Id("warning-type" )};
            this.NoResults = new SpanElement { Locator = By.Id("widgetWarnings-no-results") };
        }
    

        protected override string UrlSuffix { get; }

        public void ClickWidgetWarningTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var warningButton = btnElements.Where(x => x.Text == "Widget Warnings").FirstOrDefault();
            warningButton.Click();
        }

        public List<Grid> GetGridById(int id)
        {
            var grid = new List<WidgetWarningPage.Grid>();

            grid.Add(new WidgetWarningPage.Grid(id));

            return grid;
        }

        public class Grid
        {
            private const string LevelId = "warning-level-";

            private const string BranchesId = "warning-branch-";

            private const string TypeId = "warning-type-";

            private const string RemoveId = "warning-remove-";

            public Grid(int id)
            {
                this.Level = new SpanElement { Locator = By.Id(LevelId + id) };
                this.WidgetType = new SpanElement {Locator = By.Id(TypeId + id)};
                this.Branches = new SpanElement { Locator = By.Id(BranchesId + id) };
                this.Remove = new Button { Locator = By.Id(RemoveId + id) };
            }

            public SpanElement Level { get; set; }

            public SpanElement Branches { get; set; }

            public SpanElement WidgetType { get; set; }

            public Button Remove { get; set; }
        }
    }
}
