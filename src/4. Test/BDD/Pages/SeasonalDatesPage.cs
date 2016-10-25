namespace PH.Well.BDD.Pages
{
    using System.Collections.Generic;

    using OpenQA.Selenium;
    using PH.Well.BDD.Framework.WebElements;

    public class SeasonalDatesPage : Page
    {
        public readonly AdminButtonDropDown AdminDropDown;

        public readonly Button AddButton;

        public readonly Button SaveButton;

        public readonly TextBox Description;

        public readonly TextBox FromDate;

        public readonly TextBox ToDate;

        public readonly Button RemoveConfirmButton;

        public SpanElement NoResults;

        public SeasonalDatesPage()
        {
            this.AdminDropDown = new AdminButtonDropDown { Locator = By.Id("admin-dropdown") };
            this.AddButton = new Button { Locator = By.Id("add-seasonal-date") };
            this.Description = new TextBox { Locator = By.Id("seasonal-date-description") };
            this.FromDate = new TextBox { Locator = By.Id("seasonal-date-from-date") };
            this.ToDate = new TextBox { Locator = By.Id("seasonal-date-to-date") };
            this.SaveButton = new Button { Locator = By.Id("seasonal-date-save") };
            this.RemoveConfirmButton = new Button { Locator = By.Id("seasonal-remove-confirm") };
            this.NoResults = new SpanElement { Locator = By.Id("seasonal-no-results") };
        }

        protected override string UrlSuffix { get; }

        public List<SeasonalDateGrid> GetGridById(int id)
        {
            var grid = new List<SeasonalDateGrid>();

            grid.Add(new SeasonalDateGrid(id));

            return grid;
        }

        public List<SeasonalDateGrid> GetGrid(int rows)
        {
            var grid = new List<SeasonalDateGrid>();

            for (int i = 1; i <= rows; i++)
            {
                grid.Add(new SeasonalDateGrid(i));
            }

            return grid;
        }

        public class SeasonalDateGrid
        {
            public const string DescriptionId = "seasonal-description-";
            public const string FromDateId = "seasonal-from-date-";
            public const string ToDateId = "seasonal-to-date-";
            public const string BranchesId = "seasonal-branches-";
            public const string RemoveId = "seasonal-remove-";

            public SeasonalDateGrid(int id)
            {
                this.Description = new SpanElement { Locator = By.Id(DescriptionId + id) };
                this.FromDate = new SpanElement { Locator = By.Id(FromDateId + id) };
                this.ToDate = new SpanElement { Locator = By.Id(ToDateId + id) };
                this.Branches = new SpanElement { Locator = By.Id(BranchesId + id) };
                this.Remove = new Button { Locator = By.Id(RemoveId + id) };
            }

            public SpanElement Description { get; set; }

            public SpanElement FromDate { get; set; }

            public SpanElement ToDate { get; set; }

            public SpanElement Branches { get; set; }

            public Button Remove { get; set; }


        }
    }
}