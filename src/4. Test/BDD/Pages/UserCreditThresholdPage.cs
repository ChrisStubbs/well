namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class UserCreditThresholdPage : Page
    {
        public readonly Button SaveButton;

        public readonly Button FindButton;

        public readonly CreditThresholdButtonDropDown ThresholdLevelDropDown;

        public readonly UserThresholdNavDropDown UserThresholdNavigation;

        public SpanElement Username;

        public readonly TextBox SearchTextBox;

        public Grid<UserPreferenceGrid> Grid;

        public Div ToasterSucess;


        public UserCreditThresholdPage()
        {
            this.SaveButton = new Button { Locator = By.Id("save") };
            this.ThresholdLevelDropDown = new CreditThresholdButtonDropDown {Locator = By.Id("credit-threshold-dropdown") };
            this.UserThresholdNavigation = new UserThresholdNavDropDown { Locator = By.Id("admin-dropdown") };
            this.Username = new SpanElement {Locator = By.Id("current-user-name")};
            this.SearchTextBox = new TextBox {Locator = By.Id("user-text")};
            this.FindButton = new Button {Locator = By.Id("find-user")};
            this.Grid = new Grid<UserPreferenceGrid> { Locator = By.Id("table-user-preferences"), RowLocator = By.ClassName("grid-row") };
            this.ToasterSucess = new Div { Locator = By.Id("toast-container") };
        }

        protected override string UrlSuffix { get; }
    }
}
