namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class UserCreditThresholdPage : Page
    {
        public readonly Button SaveButton;

        public readonly Button FindButton;

        public readonly CreditThresholdButtonDropDown dropdown;

        public SpanElement Username;

        public readonly TextBox SearchTextBox;

        public Grid<UserPreferenceGrid> Grid;



        public UserCreditThresholdPage()
        {
            this.SaveButton = new Button { Locator = By.Id("save") };
            this.dropdown = new CreditThresholdButtonDropDown {Locator = By.Id("credit-threshold-dropdown") };
            this.Username = new SpanElement {Locator = By.Id("current-user-name")};
            this.SearchTextBox = new TextBox {Locator = By.Id("user-text")};
            this.FindButton = new Button {Locator = By.Id("find-user")};
            this.Grid = new Grid<UserPreferenceGrid> { Locator = By.Id("table-user-preferences"), RowLocator = By.ClassName("grid-row") };

        }

        protected override string UrlSuffix { get; }
    }
}
