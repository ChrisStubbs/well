namespace PH.Well.BDD.Pages
{
    using OpenQA.Selenium;
    using PH.Well.BDD.Framework.WebElements;

    public class BranchParametersPage : Page
    {
        public readonly AdminButtonDropDown AdminDropDown;

        public readonly Button AddButton;

        public readonly TextBox Description;

        public readonly TextBox FromDate;

        public readonly TextBox ToDate;

        public BranchParametersPage()
        {
            this.AdminDropDown = new AdminButtonDropDown { Locator = By.Id("admin-dropdown") };
            this.AddButton = new Button { Locator = By.Id("add-seasonal-date") };
            this.Description = new TextBox { Locator = By.Id("seasonal-date-description") };
            this.FromDate = new TextBox { Locator = By.Id("seasonal-date-from-date") };
            this.ToDate = new TextBox { Locator = By.Id("seasonal-date-to-date") };
        }

        protected override string UrlSuffix { get; }
    }
}