namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class UserCreditThresholdPage : Page
    {
        public readonly Button SaveButton;

        public readonly CreditThresholdButtonDropDown dropdown;

        public UserCreditThresholdPage()
        {
            this.SaveButton = new Button { Locator = By.Id("save") };
            this.dropdown = new CreditThresholdButtonDropDown {Locator = By.Id("credit-threshold-dropdown") };
        }

        protected override string UrlSuffix { get; }
    }
}
