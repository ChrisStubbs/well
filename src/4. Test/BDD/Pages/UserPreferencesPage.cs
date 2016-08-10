namespace PH.Well.BDD.Pages
{
    using OpenQA.Selenium;

    using PH.Well.BDD.Framework.WebElements;

    public class UserPreferencesPage : Page
    {
        public readonly UserPreferencesButtonDropDown UserPreferencesDropDown;

        public TextBox FindBox;

        public readonly Button FindButton;

        public readonly Button ModalYesButton;

        public Grid<UserPreferenceGrid> Grid;

        public UserPreferencesPage()
        {
            this.Grid = new Grid<UserPreferenceGrid> { Locator = By.Id("table-user-preferences"), RowLocator = By.ClassName("grid-row") };
            this.UserPreferencesDropDown = new UserPreferencesButtonDropDown { Locator = By.Id("admin-dropdown") };
            this.FindBox = new TextBox { Locator = By.Id("user-text") };
            this.FindButton = new Button { Locator = By.Id("find-user") };
            this.ModalYesButton = new Button { Locator = By.Id("modal-yes") };
        }

        protected override string UrlSuffix { get; }
    }

    public enum UserPreferenceGrid
    {
        Name = 0,
        JobDescription = 1,
        Domain = 2
    }
}