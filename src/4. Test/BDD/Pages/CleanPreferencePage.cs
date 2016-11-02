namespace PH.Well.BDD.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenQA.Selenium;

    using PH.Well.BDD.Framework.WebElements;

    public class CleanPreferencePage : Page
    {
        public readonly TextBox Days;

        public readonly Button Save;

        public readonly Button Close;

        public readonly Button Remove;

        public readonly Button Add;

        public readonly SpanElement NoResults;

        public CleanPreferencePage()
        {
            this.Days = new TextBox { Locator = By.Id("clean-days") };
            this.Save = new Button { Locator = By.Id("clean-save") };
            this.Close = new Button {Locator = By.Id("clean-close")};
            this.Remove = new Button { Locator = By.Id("clean-remove") };
            this.Add = new Button { Locator = By.Id("add-clean") };
            this.NoResults = new SpanElement { Locator = By.Id("clean-no-results") };
        }

        protected override string UrlSuffix { get; }

        public void ClickCleanDeliveriesTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var thresholdButton = btnElements.Where(x => x.Text == "Clean Deliveries").FirstOrDefault();
            thresholdButton.Click();
        }

        public void ClickSeasonalDatesTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var thresholdButton = btnElements.Where(x => x.Text == "Seasonal Dates").FirstOrDefault();
            thresholdButton.Click();
        }

        public List<string> GetErrors()
        {
            
            var elements = this.Driver.FindElements(By.ClassName("clean-error"));

            return elements.Select(element => element.Text).ToList();
        }

        public List<Grid> GetGridById(int id)
        {
            var grid = new List<Grid>();

            grid.Add(new Grid(id));

            return grid;
        }



        public class Grid
        {
            private const string DaysId = "clean-days-";

            private const string BranchesId = "clean-branches-";

            private const string RemoveId = "clean-remove-";

            public Grid(int id)
            {
                this.Days = new SpanElement { Locator = By.Id(DaysId + id) };
                this.Branches = new SpanElement { Locator = By.Id(BranchesId + id) };
                this.Remove = new Button { Locator = By.Id(RemoveId + id) };
            }


            public SpanElement Days { get; set; }

            public SpanElement Branches { get; set; }

            public Button Remove { get; set; }


        }
    }
}
