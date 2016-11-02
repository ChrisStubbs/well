using System.Security.Policy;

namespace PH.Well.BDD.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenQA.Selenium;

    using PH.Well.BDD.Framework.WebElements;

    public class CreditThresholdPage : Page
    {
        public readonly Button AddButton;

        public readonly Button SaveButton;

        public readonly TextBox Threshold;

        public readonly Button RemoveConfirmButton;

        public CreditThresholdButtonDropDown dropdown = new CreditThresholdButtonDropDown();

        public SpanElement NoResults;

        public SpanElement Username;

        public readonly AdminButtonDropDown AdminDropDown;

        public CreditThresholdPage()
        {
            this.AddButton = new Button { Locator = By.Id("add-credit-threshold") };
            this.SaveButton = new Button { Locator = By.Id("credit-threshold-save") };
            this.Threshold = new TextBox { Locator = By.Id("threshold") };
            this.RemoveConfirmButton = new Button { Locator = By.Id("threshold-remove-confirm") };
            this.NoResults = new SpanElement { Locator = By.Id("threshold-no-results") };
            this.Username = new SpanElement {Locator = By.Id("current-user-name")};
            this.AdminDropDown = new AdminButtonDropDown { Locator = By.Id("admin-dropdown") };

        }

        protected override string UrlSuffix { get; }

        public void ClickThresholdTab()
        {
            var btnElements = this.Driver.FindElements(By.ClassName("btn"));

            var thresholdButton = btnElements.Where(x => x.Text == "Credit Thresholds").FirstOrDefault();
            thresholdButton.Click();
        }

        public List<CreditThresholdGrid> GetGridById(int id)
        {
            var grid = new List<CreditThresholdGrid>();

            grid.Add(new CreditThresholdGrid(id));

            return grid;
        }

        public List<CreditThresholdGrid> GetGrid(int rows)
        {
            var grid = new List<CreditThresholdGrid>();

            for (int i = 1; i <= rows; i++)
            {
                grid.Add(new CreditThresholdGrid(i));
            }

            return grid;
        }


        public class CreditThresholdGrid
        {
            public const string LevelId = "threshold-level-";

            public const string ThresholdAmountId = "threshold-amount-";

            public const string BranchesId = "threshold-branches-";

            public const string RemoveId = "threshold-remove-";

            public CreditThresholdGrid(int id)
            {
                this.Level = new SpanElement { Locator = By.Id(LevelId + id) };
                this.ThresholdAmount = new SpanElement { Locator = By.Id(ThresholdAmountId + id) };
                this.Branches = new SpanElement { Locator = By.Id(BranchesId + id) };
                this.Remove = new Button { Locator = By.Id(RemoveId + id) };
            }

            public SpanElement Level { get; set; }

            public SpanElement ThresholdAmount { get; set; }

            public SpanElement Branches { get; set; }

            public Button Remove { get; set; }
        }
    }
}